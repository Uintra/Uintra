using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Extentions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToEnumerableOfOne<T>(this T obj)
        {
            if (obj == null)
            {
                return Enumerable.Empty<T>();
            }

            return Enumerable.Repeat<T>(obj, 1);
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}
