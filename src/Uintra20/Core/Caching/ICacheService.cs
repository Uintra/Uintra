using System;
using System.Threading.Tasks;
using LanguageExt;

namespace Uintra20.Core.Caching
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(Func<Task<T>> getItemCallback, DateTimeOffset lifetime, string cacheKey, params string[] itemSuffix) where T : class;
        Task<Option<T>> GetOrSetAsyncOption<T>(Func<Task<Option<T>>> getItemCallback, DateTimeOffset lifetime, string cacheKey, params string[] itemSuffix) where T : class;
        Task<Option<T>> GetOrSetAsync<T>(Func<Task<Option<T>>> getItemCallback, DateTimeOffset lifetime, string cacheKey, params string[] itemSuffix) where T : class;
        Task<Unit> SetAsync<T>(Func<Task<T>> getItemCallback, Func<T, DateTimeOffset> lifetime, string cacheKey, params string[] itemSuffix);

        void Set<T>(string cacheKey, T items, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes);
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes) where T : class;
        T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class;
        bool HasValue(string cacheKey, params string[] uniqueSuffixes);
        void Remove(string cacheKey, params string[] uniqueSuffixes);
    }
}
