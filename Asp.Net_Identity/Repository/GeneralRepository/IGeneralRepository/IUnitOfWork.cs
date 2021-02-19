using Repository.ProductsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GeneralRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get;  }
      
        void CommitTransaction();
        void BeginTransaction();
        void Rollback();
        void Save();
        IGenericRepository<T> GenericRepository<T>() where T : class;
    }
}
