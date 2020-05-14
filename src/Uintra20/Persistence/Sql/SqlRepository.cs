using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Context;

namespace Uintra20.Persistence.Sql
{
    public class SqlRepository<TKey, T> : ISqlRepository<TKey, T> where T : SqlEntity<TKey>
    {
        private readonly IntranetDbContext _dbContext;
        private IntranetDbContext DbContextAsync => DependencyResolver.Current.GetService<IntranetDbContext>();
        private readonly DbSet<T> _dbSet;
        private bool _disposed;

        public SqlRepository(IntranetDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = dbContext.Set<T>();
        }

        #region async

        public async Task<T> GetAsync(TKey id)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.FindAsync(id);
            }
        }

        public async Task<IList<T>> GetManyAsync(IEnumerable<TKey> ids)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                var idList = ids as IList<TKey> ?? ids.ToList();

                if (idList.IsEmpty())
                {
                    return new List<T>();
                }

                var result = set.Join(
                    idList,
                    ent => ent.Id,
                    id => id,
                    (ent, id) => ent);

                return await result.ToListAsync();
            }
        }

        public async Task<IList<T>> GetAllAsync()
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.ToListAsync();
            }
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int skip = 0, int take = Int32.MaxValue)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.Where(predicate).OrderBy(ent => ent.Id).Skip(skip).Take(take)
                                .ToListAsync();
            }
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.Where(predicate).CountAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.AnyAsync(predicate);
            }
        }

        public async Task DeleteAsync(TKey id)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                var deletingEntity = await set.FindAsync(id);
                if (deletingEntity == null)
                {
                    throw new ArgumentNullException($"{typeof(T)} entity with id = {id} doesn't exist.");
                }

                set.Remove(deletingEntity);

                await SaveAsync(context);
            }
        }

        public async Task DeleteAsync(IEnumerable<T> entities, IntranetDbContext context = null)
        {
            var isExternalContext = context != null;

            context = context ?? DbContextAsync;

            var set = context.Set<T>();

            var entityList = entities.ToList();

            entityList.ForEach(e => set.Attach(e));

            set.RemoveRange(entityList);

            await SaveAsync(context);

            if (!isExternalContext) context.Dispose();
        }

        public async Task DeleteAsync(T entity)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                set.Remove(entity);
                await SaveAsync(context);
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> deletingEntities;
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                deletingEntities = set.Where(predicate);
                await DeleteAsync(deletingEntities, context);
            }
        }

        public async Task AddAsync(T entity)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                set.Add(entity);
                await SaveAsync(context);
            }
        }

        public async Task AddAsync(IEnumerable<T> entities)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                set.AddRange(entities);
                await SaveAsync(context);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var context = DbContextAsync)
            {
                context.Entry(entity).State = EntityState.Modified;
                await SaveAsync(context);
            }
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            using (var context = DbContextAsync)
            {
                foreach (var ent in entities)
                {
                    context.Entry(ent).State = EntityState.Modified;
                }

                await SaveAsync(context);
            }
        }

        public async Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T, TProperty>> property)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                set.Attach(entity);
                context.Entry(entity).Property(property).IsModified = true;
                await SaveAsync(context);
            }
        }

        public async Task UpdatePropertyAsync<TProperty>(IEnumerable<T> entities, Expression<Func<T, TProperty>> property)
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                foreach (var entity in entities)
                {
                    set.Attach(entity);
                    context.Entry(entity).Property(property).IsModified = true;
                }
                await SaveAsync(context);
            }
        }

        public async Task<IList<T>> AsNoTrackingAsync()
        {
            using (var context = DbContextAsync)
            {
                var set = context.Set<T>();
                return await set.AsNoTracking().ToListAsync();
            }
        }

        public virtual async Task SaveAsync(IntranetDbContext context = null)
        {
            bool isExternalContext = context != null;
            context = context ?? DbContextAsync;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // for cases when we trying to update entities which does not present in database.
                var entries = context.ChangeTracker.Entries()
                    .Where(i => i.State == EntityState.Modified);
                foreach (var entry in entries)
                {
                    var dbValues = await entry.GetDatabaseValuesAsync();
                    if (dbValues == null)
                        entry.State = EntityState.Added;
                }

                await context.SaveChangesAsync();
            }
            finally
            {
                if(!isExternalContext)
                    context.Dispose();
            }
        }

        #endregion

        #region sync

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
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                // for cases when we trying to update entities which does not present in database.
                var entries = _dbContext.ChangeTracker.Entries()
                    .Where(i => i.State == EntityState.Modified);
                foreach (var entry in entries)
                {
                    var dbValues = entry.GetDatabaseValues();
                    if (dbValues == null)
                        entry.State = EntityState.Added;
                }
                _dbContext.SaveChanges();
            }
        }

        public virtual void UpdateProperty<TProperty>(T entity, Expression<Func<T, TProperty>> property)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).Property(property).IsModified = true;
            Save();
        }

        public virtual void UpdateProperty<TProperty>(IEnumerable<T> entities, Expression<Func<T, TProperty>> property)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).Property(property).IsModified = true;
            }
            Save();
        }

        public virtual IList<T> AsNoTracking()
        {
            return _dbSet.AsNoTracking().ToList();
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

        #endregion
    }

    public class SqlRepository<T> : SqlRepository<Guid, T>, ISqlRepository<T> where T : SqlEntity<Guid>
    {
        public SqlRepository(IntranetDbContext dbContext) : base(dbContext)
        {
        }
    }
}