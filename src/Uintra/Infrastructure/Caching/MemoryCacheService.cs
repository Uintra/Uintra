﻿using System;
using System.Globalization;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Uintra.Infrastructure.Caching
{
    public class MemoryCacheService : ICacheService
    {
        public Task<T> GetOrSetAsync<T>(
            Func<Task<T>> getItemCallback,
            string cacheKey,
            DateTimeOffset? lifetime = null,
            params string[] itemSuffix) where T : class
        {
            var keyPrefix = CreateKey(cacheKey, itemSuffix);

            if(MemoryCache.Default.Get(keyPrefix) is T result)
                return Task.FromResult(result);
            
            var item = getItemCallback();
            Set(cacheKey, item, lifetime, itemSuffix);

            return item;
        }
        public async Task SetAsync<T>(Func<Task<T>> getItemCallback, string cacheKey, DateTimeOffset? lifetime = null, params string[] itemSuffix)
        {
            var key = CreateKey(cacheKey, itemSuffix);
            var item = await getItemCallback();

            var itemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = lifetime ?? ObjectCache.InfiniteAbsoluteExpiration
            };
            itemPolicy.ChangeMonitors.Add(new SignaledChangeMonitor(key));
            MemoryCache.Default.Set(key, item, itemPolicy);
        }

        public T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            var item = MemoryCache.Default.Get(key) as T;
            return item;
        }

        public bool HasValue(string cacheKey, params string[] uniqueSuffixes)
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            return MemoryCache.Default.Contains(key);
        }

        public void Set<T>(string cacheKey, T item, DateTimeOffset? lifeTime = null, params string[] uniqueSuffixes)
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            var itemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = lifeTime ?? ObjectCache.InfiniteAbsoluteExpiration
            };
            itemPolicy.ChangeMonitors.Add(new SignaledChangeMonitor(key));
            MemoryCache.Default.Set(key, item, itemPolicy);
        }

        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset? lifeTime = null,
            params string[] uniqueSuffixes) where T : class
        {
            var keyPrefix = CreateKey(cacheKey, uniqueSuffixes);
            var item = MemoryCache.Default.Get(keyPrefix) as T;
            if (item == null)
            {
                item = getItemCallback();
                Set(cacheKey, item, lifeTime, uniqueSuffixes);
            }

            return item;
        }

        public void Remove(string cacheKey, params string[] uniqueSuffixes)
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            MemoryCache.Default.Remove(key);
            //SignaledChangeMonitor.Signal(cacheKey);
        }

        private static string CreateKey(string cacheKey, string[] uniqueSuffixes)
        {
            var key = $"{cacheKey}:[{string.Join("][", uniqueSuffixes)}]";
            return key;
        }

        private class SignaledChangeMonitor : ChangeMonitor
        {
            // Shared across all SignaledChangeMonitors in the AppDomain
            private static event EventHandler<SignaledChangeEventArgs> Signaled;

            private readonly string name;

            public override string UniqueId { get; } = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            public SignaledChangeMonitor(string name = null)
            {
                this.name = name;
                // Register instance with the shared event
                Signaled += OnSignalRaised;
                InitializationComplete();
            }

            protected override void Dispose(bool disposing)
            {
                Signaled -= OnSignalRaised;
            }

            public static void Signal(string name = null)
            {
                if (Signaled != null)
                {
                    // Raise shared event to notify all subscribers
                    Signaled(null, new SignaledChangeEventArgs(name));
                }
            }

            private void OnSignalRaised(object sender, SignaledChangeEventArgs e)
            {
                if (string.IsNullOrEmpty(e.Name) || string.Equals(e.Name, name))
                {
                    // Cache objects are obligated to remove entry upon change notification.
                    OnChanged(null);
                }
            }

            private class SignaledChangeEventArgs : EventArgs
            {
                public string Name { get; }

                public SignaledChangeEventArgs(string name = null)
                {
                    Name = name;
                }
            }
        }
    }
}