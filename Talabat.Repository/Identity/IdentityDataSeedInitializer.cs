using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Repository.Identity
{
	public static class IdentityDataSeedInitializer
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					DisplayName = "Abdalrhman Gamal",
					Email = "AbdalrhmanGamal681@gmail.com",
					UserName = "AbdalrhmanGamal681",
					PhoneNumber = "01067377533",
				};

				await userManager.CreateAsync(user, "Pa$$w0rd");
			}
		}
	}
}
