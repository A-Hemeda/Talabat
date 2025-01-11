using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
		public ProductItemOrdered Product { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		#region Constructors
		public OrderItem() { }

		public OrderItem(ProductItemOrdered product, decimal price, int quantity)
		{
			Product = product;
			Price = price;
			Quantity = quantity;
		}
		#endregion
	}
}
