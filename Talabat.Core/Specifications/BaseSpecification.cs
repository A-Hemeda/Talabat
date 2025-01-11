using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
	public abstract class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
	{
		#region Properties
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderByDescending { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public bool IsPaginationEnabled { get; set; }
		#endregion

		#region Constructors
		// GetAll endpoint (We Don't Need AnyWhere Condition)
		public BaseSpecification() { }

		// GetByID endpoint (We Need Where Condition)
		public BaseSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}
		#endregion

		#region Methods
		// Protected method so whoever inherits the class can push all of its includes into the list
		protected void AddIncludes(Expression<Func<T, object>> includeExpression)
		{
			Includes.Add(includeExpression);
		}

		// Ordering Ascending based on a specific property
		protected void AddOrderBy(Expression<Func<T, object>> orderBy)
		{
			OrderBy = orderBy;
		}

		// Ordering Descending based on a specific property
		protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
		{
			OrderByDescending = orderByDescending;
		}

		// Determine Number Of Items To Skip & Number Of Items To Take
		protected void ApplyPagination(int skip, int take)
		{
			IsPaginationEnabled = true;
			Skip = skip;
			Take = take;
		}
		#endregion
	}
}
