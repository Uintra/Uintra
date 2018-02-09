using System.Collections.Generic;
using System.Linq;
using Extensions;

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

        /// <summary>Adds elements at the end of sequence.</summary>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source,  T element)
        {
            return source.Concat(element.ToEnumerable());
        }
    }
}
