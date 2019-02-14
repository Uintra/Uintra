using System;

namespace Uintra.Core.Caching
{
    public interface ICacheService
    {
        void Set<T>(string cacheKey, T items, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes);
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes) where T : class;
        T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class;
        bool HasValue(string cacheKey, params string[] uniqueSuffixes);
    }
}