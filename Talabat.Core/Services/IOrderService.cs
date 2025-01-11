using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Services
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
		Task<Order> GetOrderByIdAsync(string buyerEmail, int orderId);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
	}
}
