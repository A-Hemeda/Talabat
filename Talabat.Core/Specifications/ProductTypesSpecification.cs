using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
	public class ProductTypesSpecification : BaseSpecification<ProductType>
	{
		public ProductTypesSpecification(string? sort) : base()
		{
			// Ordering Ascending or Descending based on a specific property
			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
				{
					case "IdAsc":
						AddOrderBy(p => p.Id);
						break;

					case "IdDesc":
						AddOrderByDescending(p => p.Id);
						break;

					case "NameAsc":
						AddOrderBy(p => p.Name);
						break;

					case "NameDesc":
						AddOrderByDescending(p => p.Name);
						break;

					default:
						AddOrderBy(p => p.Id);
						break;
				}
			}
		}

		public ProductTypesSpecification(int id) : base(t => t.Id == id) { }
	}
}
