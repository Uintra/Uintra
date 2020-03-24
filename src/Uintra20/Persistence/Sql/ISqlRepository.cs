using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uintra20.Persistence.Context;

namespace Uintra20.Persistence.Sql
{
    public interface ISqlRepository<TKey, T> : IDisposable
    {
        IQueryable<T> AsQueryable();

        T Get(TKey id);

        IList<T> GetMany(IEnumerable<TKey> ids);

        IList<T> GetAll();

        T Find(Expression<Func<T, bool>> predicate);

        IList<T> FindAll(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue);

        long Count(Expression<Func<T, bool>> predicate);

        bool Exists(Expression<Func<T, bool>> predicate);

        void Delete(TKey id);

        void Delete(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void Add(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void UpdateProperty<TProperty>(T entity, Expression<Func<T, TProperty>> property);

        void UpdateProperty<TProperty>(IEnumerable<T> entity, Expression<Func<T, TProperty>> property);

        IList<T> AsNoTracking();



        Task<T> GetAsync(TKey id);

        Task<IList<T>> GetManyAsync(IEnumerable<TKey> ids);

        Task<IList<T>> GetAllAsync();

        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IList<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(TKey id);

        Task DeleteAsync(IEnumerable<T> entities, IntranetDbContext context = null);

        Task DeleteAsync(T entity);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task AddAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateAsync(IEnumerable<T> entities);

        Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T, TProperty>> property);

        Task UpdatePropertyAsync<TProperty>(IEnumerable<T> entity, Expression<Func<T, TProperty>> property);

        Task<IList<T>> AsNoTrackingAsync();
    }

    public interface ISqlRepository<T> : ISqlRepository<Guid, T> where T : SqlEntity<Guid>
    {
    }
}
