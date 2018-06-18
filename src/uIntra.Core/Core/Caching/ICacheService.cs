using System;
using System.Linq.Expressions;

namespace uIntra.Core.Caching
{
    public interface ICacheService
    {
        void Clear<T>(Expression<Func<T>> getItemCallback, params string[] uniqueSuffixes) where T : class;
        void Clear(string cacheKey, params string[] uniqueSuffixes);
        void Flush();
        void Set<T>(string cacheKey, T items, DateTimeOffset lifeTime, params string[] uniqueSuffixes);
        T GetOrSet<T>(Expression<Func<T>> getItemCallback, int lifeTimeInMinutes, params string[] uniqueSuffixes) where T : class;
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset lifeTime, params string[] uniqueSuffixes) where T : class;
        T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class;
        bool HasValue(string cacheKey, params string[] uniqueSuffixes);
    }
}