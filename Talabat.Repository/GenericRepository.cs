using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly API1DbContext _dbContext;

        public GenericRepository(API1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Without Specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity)
            => await _dbContext.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);

        public void Update(T item)
            => _dbContext.Set<T>().Update(item);
        #endregion

        #region With Specification
        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).ToListAsync();

        public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
            => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        #endregion
    }
}
