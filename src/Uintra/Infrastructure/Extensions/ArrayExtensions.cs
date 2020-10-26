using System;

namespace Uintra.Infrastructure.Extensions
{
    public static class ArrayExtensions
    {
        public static void ForEach<T>(this T[] src, Action<T> action) => 
            Array.ForEach(src, action);
    }
}