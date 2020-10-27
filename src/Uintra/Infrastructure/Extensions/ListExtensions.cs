using System.Collections.Generic;

namespace Uintra.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ToListOfOne<T>(this T obj) =>
            obj == null ? new List<T>() : new List<T> { obj };
    }
}