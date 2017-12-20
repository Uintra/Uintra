using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using uIntra.Core.Extensions;

namespace uIntra.Core.Persistence
{
    public class SqlRepository<TKey, T> : ISqlRepository<TKey, T> where T : SqlEntity<TKey>
    {
        private readonly IntranetDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private bool _disposed;

        public SqlRepository(IntranetDbContext dbContext)
        {   
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> AsQueryable() => _dbSet;

        public IList<T> GetAll() => _dbSet.ToList();

        public T Get(TKey id) => _dbSet.Find(id);

        public T Find(Expression<Func<T, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

        public IList<T> FindAll(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue) =>
            _dbSet.Where(predicate).OrderBy(ent => ent.Id).Skip(skip).Take(take).ToList();

        public IList<T> GetMany(IEnumerable<TKey> ids)
        {
            var idList = ids as IList<TKey> ?? ids.ToList();

            if (idList.IsEmpty())
            {
                return new List<T>();
            }

            var result = _dbContext.Set<T>().Join(
                idList,
                ent => ent.Id,
                id => id,
                (ent, id) => ent);

            return result.ToList();
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

        public void Delete(TKey id)
        {
            var deletingEntity = _dbSet.Find(id);
            if (deletingEntity == null)
            {
                throw new ArgumentNullException($"{typeof(T)} entity with id = {id} doesn't exist.");
            }

            _dbSet.Remove(deletingEntity);

            Save();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var deletingEntities = _dbSet.Where(predicate);
            Delete(deletingEntities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            Save();
        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
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
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }

    public class SqlRepository<T> : SqlRepository<Guid, T>, ISqlRepository<T> where T : SqlEntity<Guid>
    {
        public SqlRepository(IntranetDbContext dbContext) : base(dbContext)
        {
        }
    }
}