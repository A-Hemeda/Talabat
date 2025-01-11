using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
	public class ProductSpecificationParams
	{
		public string? Sort { get; set; }
		public int? BrandId { get; set; }
		public int? TypeId { get; set; }

		private int pageSize = 5; // Default Page Size Is 5
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > 10 ? 10 : value; } // Check If The Page Size Is More That 10 Items
		}

		public int PageNumber { get; set; } = 1; // Default Page Number Is 1
	}
}
