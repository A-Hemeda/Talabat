using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositories
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		// Generic method that returns a repository for a given entity type T (Order, Product,...)
		IGenericRepository<T> Repository<T>() where T : BaseEntity;
		Task<int> CompleteAsync();
	}
}
