using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using uIntra.Core.Extensions;

namespace uIntra.Core.Persistence
{
    public class SqlRepository<TKey, T> : ISqlRepository<TKey, T> where T : SqlEntity<TKey>
    {
        private readonly IntranetDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private bool disposed;

        public SqlRepository(IntranetDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();

        }

        public T Get(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetMany(IEnumerable<TKey> ids)
        {
            if (ids.IsEmpty())
            {
                return Enumerable.Empty<T>();
            }

            var result = _dbContext.Set<T>().Join(
                ids,
                ent => ent.Id,
                id => id,
                (ent, id) => ent);

            return result;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IEnumerable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue)
        {
            return _dbSet.Where(predicate).OrderBy(ent => ent.Id).Skip(skip).Take(take).ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);

            Save();
        }

        public void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);

            Save();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            Save();
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var ent in entities)
            {
                _dbContext.Entry(ent).State = EntityState.Modified;
            }

            Save();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);

            Save();
        }

        public void DeleteById(TKey id)
        {
            var deletingEntity = _dbSet.Find(id);
            if (deletingEntity == null)
            {
                throw new ArgumentNullException($"{typeof(T)} entity with id = {id} doesn't exist.");
            }

            _dbSet.Remove(deletingEntity);

            Save();
        }

        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var deletingEntities = _dbSet.Where(predicate);
            Delete(deletingEntities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            Save();
        }

        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        public bool Exists(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Any();
        }

        public virtual void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

    public class SqlRepository<T> : SqlRepository<Guid, T>, ISqlRepository<T> where T : SqlEntity<Guid>
    {
        public SqlRepository(IntranetDbContext dbContext) : base(dbContext)
        {
        }
    }
}