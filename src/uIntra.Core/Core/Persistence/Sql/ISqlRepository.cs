using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace uIntra.Core.Persistence
{
    public interface ISqlRepository<TKey, T> : IDisposable
    {
        T Get(TKey id);

        IEnumerable<T> GetMany(IEnumerable<TKey> ids);

        IEnumerable<T> GetAll();

        T Find(Expression<Func<T, bool>> predicate);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue);

        long Count(Expression<Func<T, bool>> predicate);

        bool Exists(Expression<Func<T, bool>> predicate);

        void DeleteById(TKey id);

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