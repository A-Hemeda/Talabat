using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
	public class OrderSpecifications : BaseSpecification<Order>
	{
		public OrderSpecifications(string email) 
			: base(order => order.BuyerEmail == email)
		{
			AddIncludes(order => order.DeliveryMethod);
			AddIncludes(order => order.Items);

			AddOrderByDescending(order => order.OrderDate);
		}

		public OrderSpecifications(string email, int orderId) 
			: base(order => order.BuyerEmail == email && order.Id == orderId) 
		{
			AddIncludes(order => order.DeliveryMethod);
			AddIncludes(order => order.Items);
		}
	}
}
