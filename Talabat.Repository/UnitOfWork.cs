using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly API1DbContext _dbContext;
		private Hashtable _repositories; // Store repositories for different entity types [Key:Entity, Value:Repository]

		public UnitOfWork(API1DbContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}

		public IGenericRepository<T> Repository<T>() where T : BaseEntity
		{
			var type = typeof(T).Name; // Key => Product, Order,...

			if (!_repositories.ContainsKey(type))
			{
				var repository = new GenericRepository<T>(_dbContext); // Value
				_repositories.Add(type, repository);
			}

			return (IGenericRepository<T>)_repositories[type];
		}

		public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();
	}
}
