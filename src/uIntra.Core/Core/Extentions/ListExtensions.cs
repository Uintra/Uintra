using System.Collections.Generic;

namespace uIntra.Core.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ToListOfOne<T>(this T obj)
        {
            if (obj == null)
            {
                return new List<T>();
            }
            return new List<T> { obj };
        }
    }
}
