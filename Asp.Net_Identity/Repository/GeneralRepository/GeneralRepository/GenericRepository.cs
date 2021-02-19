using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using DataConnection;

namespace Repository.GeneralRepository
{
   public class GenericRepository<T> : IGenericRepository<T> where T : class
   {
        internal AppDbContext context;
        internal DbSet<T> dbSet;
        private string _errorMessage = string.Empty;


       


        public GenericRepository(DataConnection.AppDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetWithRawSql(string query,
            params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).ToList();
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual T GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            dbSet.Add(entity);

            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                dbSet.Add(entity);
                
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void AddRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Set<T>().AddRange(entities);
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                             validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }






        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);


            try
            {
                if (entityToDelete == null)
                    throw new ArgumentNullException("entity");
                dbSet.Remove(entityToDelete);
                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public virtual void Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;

            try
            {
                if (entityToUpdate == null)
                    throw new ArgumentNullException("entity");
              
                SetEntryModified(entityToUpdate);
                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public virtual void SetEntryModified(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
