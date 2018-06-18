using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Uintra.Core.Persistence
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
    }

    public interface ISqlRepository<T> : ISqlRepository<Guid, T> where T : SqlEntity<Guid>
    {
    }
}