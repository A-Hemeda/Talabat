using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Extension_Methods;
using Talabat.Core.Models;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenServices tokenServices, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }

        #region EndPoints

        #region POST: BaseUrl/api/Account/Register
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ErrorResponse(400, "The Email Is Already Exists"));

            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ErrorResponse(400));

            var registeredUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };

            return Ok(registeredUser);
        }
        #endregion

        #region POST: BaseUrl/api/Account/Login
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Unauthorized(new ErrorResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ErrorResponse(401));

            var loginUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };

            return Ok(loginUser);
        }
        #endregion

        #region GET: BaseUrl/api/Account/CurrentUser
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email); // To get Current User Email

            var user = await _userManager.FindByEmailAsync(email);

            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };

            return Ok(returnedUser);
        }
        #endregion

        #region GET: BaseUrl/api/Account/CurrentUserAddress
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("CurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedUserAddress = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(mappedUserAddress);
        }
        #endregion

        #region PUT: BaseUrl/api/Account/Address
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<IdentityResult>> UpdateAddress(AddressDto addressToUpdate)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            if (user is null)
                return Unauthorized(new ErrorResponse(401));

            var address = _mapper.Map<AddressDto, Address>(addressToUpdate);

            address.Id = user.Address.Id; // To Update On The Same User

            user.Address = address;

            var updatedAddress = await _userManager.UpdateAsync(user);

            if (!updatedAddress.Succeeded)
                return BadRequest(new ErrorResponse(400));

            return Ok(updatedAddress);
        }
        #endregion

        #region GET: BaseUrl/api/Account/EmailExists
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        #endregion

        #endregion
    }
}
