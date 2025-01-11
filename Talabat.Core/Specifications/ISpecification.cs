using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
	// Signature For Prosperity For Each Component Of Query
	public interface ISpecification<T> where T : BaseEntity
	{
		// Signature For Prosperity For Where Condition
		public Expression<Func<T, bool>> Criteria { get; set; }

		// Signature For Prosperity For List Of Includes
		public List<Expression<Func<T, object>>> Includes { get; set; }

		// Signature For Prosperity For Order By Ascending
		public Expression<Func<T, object>> OrderBy { get; set; }

		// Signature For Prosperity For Order By Descending
		public Expression<Func<T, object>> OrderByDescending { get; set; }

		// Number Of Items To Skips Based On The Page
		public int Skip { get; set; }

		// Number Of Items To Takes only For The Current Page
		public int Take { get; set; }

		// To Determine If Pagination Enabled Or Not
		public bool IsPaginationEnabled { get; set; }
	}
}
