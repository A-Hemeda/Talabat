using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }

		#region Relations
		// We will configure the relations using fluent api in Context class

		public int ProductBrandId { get; set; }
		public ProductBrand ProductBrand { get; set; }

		public int ProductTypeId { get; set; }
		public ProductType ProductType { get; set; }
		#endregion
	}
}
