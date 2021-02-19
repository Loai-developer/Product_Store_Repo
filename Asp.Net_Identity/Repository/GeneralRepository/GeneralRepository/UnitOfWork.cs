using DataConnection;
using Repository.GeneralRepository;
using Repository.ProductsRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GeneralRepository
{
  public  class UnitOfWork:IUnitOfWork
    {
        private AppDbContext _Context { get; }
        private IProductRepository _ProductsRepository;


        private bool _disposed;
        private string _errorMessage = string.Empty;
        private DbContextTransaction _objTran;
        private Dictionary<string, object> _repositories;


        public UnitOfWork(AppDbContext dbContext)
        {
            _Context = dbContext;
        }
     
        public IProductRepository Products
        {
            get
            {
                return _ProductsRepository ??
                    (_ProductsRepository = new ProductRepository(_Context));
            }
        }

       

        public void CommitTransaction()
        {
            _objTran.Commit();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _objTran = _Context.Database.BeginTransaction();
        }

        public void Save()
        {
            try
            {
                _Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _Context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<T>)_repositories[type];
        }



       
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _Context.Dispose();
            _disposed = true;
        }

      


    }
}
