using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
	public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
	{
		// Used For GetAll Products
		public ProductWithBrandAndTypeSpecification(ProductSpecificationParams productParams)
			: base(p =>
			(!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
			(!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId))
		{
			AddIncludes(P => P.ProductType);
			AddIncludes(P => P.ProductBrand);

			if (!string.IsNullOrEmpty(productParams.Sort))
			{
				switch (productParams.Sort)
				{
					case "IdAsc":
						AddOrderBy(p => p.Id);
						break;

					case "IdDesc":
						AddOrderByDescending(p => p.Id);
						break;

					case "PriceAsc":
						AddOrderBy(p => p.Price);
						break;

					case "PriceDesc":
						AddOrderByDescending(p => p.Price);
						break;

					default:
						AddOrderBy(p => p.Name);
						break;
				}
			}

			// Skips a certain set of records, by the page number * page size.
			// Takes only the required amount of data, set by page size.
			ApplyPagination(productParams.PageSize * (productParams.PageNumber - 1), productParams.PageSize);
		}

		// Used For Get Product By Id
		public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
		{
			AddIncludes(P => P.ProductType);
			AddIncludes(P => P.ProductBrand);
		}
	}
}
