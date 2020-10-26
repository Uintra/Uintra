using System;
using System.Threading.Tasks;

namespace Uintra20.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(
            Func<Task<T>> getItemCallback,
            string cacheKey,
            DateTimeOffset? lifetime = null,
            params string[] itemSuffix) where T : class;
        Task SetAsync<T>(Func<Task<T>> getItemCallback, string cacheKey, DateTimeOffset? lifetime = null, params string[] itemSuffix);

        void Set<T>(string cacheKey, T items, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes);
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes) where T : class;
        T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class;
        bool HasValue(string cacheKey, params string[] uniqueSuffixes);
        void Remove(string cacheKey, params string[] uniqueSuffixes);
    }
}
