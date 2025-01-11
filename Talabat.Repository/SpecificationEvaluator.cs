using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
	public static class SpecificationEvaluator<T> where T : BaseEntity
	{
		// Function To Build Query
		public static IQueryable<T> GetQuery(IQueryable<T> dbSet, ISpecification<T> specification)
		{
			// dbContext.Products
			var query = dbSet;

			// Where(P => P.Id == id)
			if (specification.Criteria is not null)
				query = query.Where(specification.Criteria);

			// OrderBy(P => P.Property) 
			if (specification.OrderBy is not null)
				query = query.OrderBy(specification.OrderBy);

			// OrderByDescending(P => P.Property) 
			if (specification.OrderByDescending is not null)
				query = query.OrderByDescending(specification.OrderByDescending);

			// Skip(n).Take(n)
			if (specification.IsPaginationEnabled)
				query = query.Skip(specification.Skip).Take(specification.Take);

			// Include(p => p.ProductType).Include(p => p.ProductBrand)
			query = specification.Includes
				.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

			return query;
		}
	}
}
