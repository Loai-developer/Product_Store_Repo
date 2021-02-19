using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GeneralRepository
{
    public interface IGenericRepository<T>  :IRepository where T : class
    {
        void Delete(T entityToDelete);
        void Delete(object id);
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        T GetByID(object id);
        IEnumerable<T> GetWithRawSql(string query,
            params object[] parameters);
        void Insert(T entity);
        void Update(T entityToUpdate);

        void AddRange(IEnumerable<T> entities);
    }
}
