using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace uCommunity.Core.Persistence.Sql
{
    public class SqlRepository<T> : ISqlRepository<T> where T : class, new()
    {
        private readonly IDbConnectionFactory _dbConnection;

        public SqlRepository(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public T Get(object id)
        {
            return Read(connection => connection.SingleById<T>(id));
        }

        public IEnumerable<T> GetMany(IEnumerable<object> ids)
        {
            return Read(connection => connection.SelectByIds<T>(ids));
        }

        public IEnumerable<T> GetAll()
        {
            return Read(connection => connection.Select<T>());
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return Read(connection => connection.Single(predicate));
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, int skip = 0, int take = int.MaxValue)
        {
            return Read(conn =>
            {
                var query = conn.From<T>().Where(predicate).Limit(skip, take);
                return conn.Select(query);
            });
        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            return Read(connection => connection.Count(predicate));
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Read(connection => connection.Exists(predicate));
        }

        public void DeleteById(object id)
        {
            Execute(conn => conn.DeleteById<T>(id));
        }

        public void Delete(IEnumerable<T> entities)
        {
            Execute(conn => conn.DeleteAll(entities));
        }

        public void Delete(T entity)
        {
            Execute(conn => conn.Delete(entity));
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Execute(conn => conn.Delete(predicate));
        }

        public void Add(T entity)
        {
            Execute(conn => conn.Insert(entity));
        }

        public void Add(IEnumerable<T> entities)
        {
            Execute(conn => conn.InsertAll(entities));
        }

        public void Update(T entity)
        {
            Execute(conn => conn.Update(entity));
        }

        public void Update(IEnumerable<T> entities)
        {
            Execute(conn => conn.UpdateAll(entities));
        }

        private void Execute(Action<IDbConnection> action)
        {
            using (var conn = _dbConnection.Open())
            {
                action(conn);
            }
        }

        private TValue Read<TValue>(Func<IDbConnection, TValue> read)
        {
            using (var conn = _dbConnection.Open())
            {
                return read(conn);
            }
        }
    }
}