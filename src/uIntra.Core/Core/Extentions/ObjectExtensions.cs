using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Extentions
{
    public static class ObjectExtensions
    {
        public static bool In<T>(this T value, params T[] items)
        {
            if (items == null || (object)value == null)
                return false;
            return ((IEnumerable<T>) items).Contains<T>(value);
        }

        // Allows you to chain your calls instead of f(g(h()))
        public static TResult Map<T, TResult>(this T t, Func<T, TResult> f) => f(t);
    }
}
