using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		#region Without Specification
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task AddAsync(T entity);
		void Delete(T entity);
		void Update(T item);
        #endregion

        #region With Specification
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
		Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification);
		#endregion
	}
}
