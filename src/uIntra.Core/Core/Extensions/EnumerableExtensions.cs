using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Extensions
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
    }
}
