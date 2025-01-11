using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Order_Aggregate
{
	public class ProductItemOrdered
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string PictureUrl { get; set; }

		#region Constructors
		public ProductItemOrdered() { }

		public ProductItemOrdered(int productId, string productName, string pictureUrl)
		{
			ProductId = productId;
			ProductName = productName;
			PictureUrl = pictureUrl;
		}
		#endregion
	}
}
