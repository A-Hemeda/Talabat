using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models;

namespace Talabat.APIS.Extension_Methods
{
	public static class UserMangerExtensions
	{
		public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal principal)
		{
			var email = principal.FindFirstValue(ClaimTypes.Email);

			var user = await userManager.Users
				.Include(u => u.Address)
				.FirstOrDefaultAsync(u => u.Email == email);

			return user;
		}
	}
}
