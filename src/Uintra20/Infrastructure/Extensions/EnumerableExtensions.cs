using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uintra20.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int divider = 2)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / divider)
                .Select(x => x.Select(v => v.Value));
        }

        public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;

        public static IEnumerable<TResult> TryCast<TResult>(this IEnumerable source)
        {
            foreach (var item in source)
            {
                if (item is TResult castedItem)
                {
                    yield return castedItem;
                }
            }
        }

        public static IEnumerable<T> WithUpdatedElement<T, TIdentity>(
            this IEnumerable<T> source,
            Func<T, TIdentity> identitySelector,
            T element)
        {
            var elementIdentity = identitySelector(element);

            return source.Where(e => !Equals(identitySelector(e), elementIdentity))
                .Append(element).ToList();
        }
        
        public static TValue ItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.ContainsKey(key) ? dictionary[key] : default(TValue);

        public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2) =>
            source1.SelectMany(x => source2.Select(y => (x, y)));

        public static Task<TResult[]> SelectAsync<T, TResult>(
            this IEnumerable<T> source,
            Func<T, Task<TResult>> selector)
        {
            return Task.WhenAll(source.Select(async s => await selector(s)));
        }

        public static async Task<IEnumerable<TResult>> SelectManyAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<IEnumerable<TResult>>> selector)
        {
            var result = await Task.WhenAll(source.Select(async s => await selector(s)));

            return result.SelectMany(x => x);
        }

        public static async Task<IEnumerable<TResult>> SelectManyAsync<T, TResult>(this IEnumerable<T> source,
            Func<T, Task<TResult[]>> selector)
        {

            var result = await Task.WhenAll(source.Select(async s => await selector(s)));

            return result.SelectMany(x => x);
        }

        public static IEnumerable<T> ToEnumerableOfOne<T>(this T obj)
        {
            return obj == null 
                ? Enumerable.Empty<T>() 
                : Enumerable.Repeat<T>(obj, 1);
        }
    }
}