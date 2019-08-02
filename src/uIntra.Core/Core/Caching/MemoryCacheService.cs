using System;
using System.Globalization;
using System.Runtime.Caching;

namespace Uintra.Core.Caching
{
    public class MemoryCacheService : ICacheService
    {
        public T Get<T>(string cacheKey, params string[] uniqueSuffixes) where T : class => 
            MemoryCache.Default.Get(CreateKey(cacheKey, uniqueSuffixes)) as T;

        public bool HasValue(string cacheKey, params string[] uniqueSuffixes) => 
            MemoryCache.Default.Contains(CreateKey(cacheKey, uniqueSuffixes));

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

        public T GetOrSet<T>(
            string cacheKey, 
            Func<T> getItemCallback, 
            DateTimeOffset? lifeTime = null, 
            params string[] uniqueSuffixes) where T : class
        {
            var item = MemoryCache.Default.Get(CreateKey(cacheKey, uniqueSuffixes)) as T;

            if (item == null)
            {
                item = getItemCallback();
                Set(cacheKey, item, lifeTime, uniqueSuffixes);
            }

            return item;
        }

        public void Remove(string cacheKey, params string[] uniqueSuffixes) => 
            MemoryCache.Default.Remove(CreateKey(cacheKey, uniqueSuffixes));

        private static string CreateKey(string cacheKey, string[] uniqueSuffixes) => 
            $"{cacheKey}:[{string.Join("][", uniqueSuffixes)}]";

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

            protected override void Dispose(bool disposing) => 
                Signaled -= OnSignalRaised;

            // Raise shared event to notify all subscribers
            public static void Signal(string name = null) => 
                Signaled?.Invoke(null, new SignaledChangeEventArgs(name));

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

                public SignaledChangeEventArgs(string name = null) => 
                    Name = name;
            }
        }

    }
}