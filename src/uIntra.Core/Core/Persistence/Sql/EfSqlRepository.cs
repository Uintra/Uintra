using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using uIntra.Core.Persistence;
using EntityState = System.Data.Entity.EntityState;

namespace uIntra.Core.Core.Persistence.EF
{
    public class EfSqlRepository<T> : ISqlRepository<T> where T : class, new()
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private bool disposed;

        public EfSqlRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var deletingEntities = _dbSet.Where(predicate);
            _dbSet.RemoveRange(deletingEntities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void DeleteById(object id)
        {

            // _dbContext.Set<T>().Remove(id);
        }

        public bool Exists(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Any();
        }

        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Find(predicate);
        }

        public IEnumerable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue)
        {
            return _dbSet.Where(predicate).Skip(skip).Take(take);
        }

        public T Get(object id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public IEnumerable<T> GetMany(IEnumerable<object> ids)
        {

            _dbContext.Set<T>().Join()
            //return  _dbContext.Set<T>().(ids);
        }

        public void Update(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AsNoTracking().;
            //   EFBatchOperation.For(_dbContext,)
            _dbContext.Set<T>().Update(entities);
            //  _dbContext.Entry<T>(entities).State = EntityState.Modified;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _dbContext.Dispose();
            }

            disposed = true;
        }
    }
}
