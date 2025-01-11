using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositories
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetBasketAsync(string basketId);
		Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket); // Create Or Update
		Task<bool> DeleteBasketAsync(string basketId);
	}
}
