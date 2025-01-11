using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.APIS.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        #region EndPoints

        #region GET: BaseUrl/api/Basket
        // Get Or  Recreate Basket
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return (basket is null) ? new CustomerBasket(basketId) : basket;
        }
        #endregion

        #region POST: BaseUrl/api/Basket
        // Update Or Create New Basket
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var createdOrUpdated = await _basketRepository.UpdateBasketAsync(mappedBasket);

            return (createdOrUpdated is null) ? BadRequest(new ErrorResponse(400)) : Ok(createdOrUpdated);
        }
        #endregion

        #region DELETE: BaseUrl/api/Basket
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }
        #endregion

        #endregion
    }
}
