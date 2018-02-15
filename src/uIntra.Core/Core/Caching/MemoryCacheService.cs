using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace Uintra.Core.Caching
{
    public class MemoryCacheService : ICacheService
    {
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

        public void Set<T>(string cacheKey, T item, DateTimeOffset lifeTime, params string[] uniqueSuffixes)
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            var itemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = lifeTime
            };
            itemPolicy.ChangeMonitors.Add(new SignaledChangeMonitor(key));
            MemoryCache.Default.Set(key, item, itemPolicy);
        }

        public T GetOrSet<T>(Expression<Func<T>> getItemCallback, int lifeTimeInMinutes, params string[] uniqueSuffixes) where T : class
        {
            var cacheKey = MakeCacheKey(getItemCallback);
            return GetOrSet(cacheKey, getItemCallback.Compile(), DateTimeOffset.Now.AddMinutes(lifeTimeInMinutes), uniqueSuffixes);
        }

        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTimeOffset lifeTime, params string[] uniqueSuffixes) where T : class
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            var item = MemoryCache.Default.Get(key) as T;
            if (item == null)
            {
                item = getItemCallback();
                Set(cacheKey, item, lifeTime, uniqueSuffixes);
            }
            return item;
        }

        public void Clear<T>(Expression<Func<T>> getItemCallback, params string[] uniqueSuffixes) where T : class
        {
            Clear(MakeCacheKey(getItemCallback), uniqueSuffixes);
        }

        public void Clear(string cacheKey, params string[] uniqueSuffixes)
        {
            var key = CreateKey(cacheKey, uniqueSuffixes);
            SignaledChangeMonitor.Signal(key);
        }

        public void Flush()
        {
            SignaledChangeMonitor.Signal();
        }

        private static string MakeCacheKey<T>(Expression<Func<T>> getItemCallback) where T : class
        {
            var key = $"{getItemCallback.ReturnType.Name}:{GetMemberName(getItemCallback.Body)}";
            return key;
        }

        private static string CreateKey(string cacheKey, string[] uniqueSuffixes)
        {
            var key = $"{cacheKey}:[{string.Join("][", uniqueSuffixes)}]";
            return key;
        }

        private static string GetMemberName(Expression expression)
        {
            return (expression as MemberExpression)?.Member.Name
                   ?? (expression as MethodCallExpression)?.Method.Name
                   ?? ((expression as UnaryExpression)?.Operand as MethodCallExpression)?.Method.Name
                   ?? ((expression as UnaryExpression)?.Operand as MemberExpression)?.Member.Name;
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

            public static void Signal(string name = null)
            {
                // Raise shared event to notify all subscribers
                Signaled?.Invoke(null, new SignaledChangeEventArgs(name));
            }

            protected override void Dispose(bool disposing)
            {
                Signaled -= OnSignalRaised;
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