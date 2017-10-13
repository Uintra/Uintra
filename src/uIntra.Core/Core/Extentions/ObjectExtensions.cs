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

        public static int? TryCastToInt(this object value) => 
            value is int converted ? converted : default;

        // Allows you to chain your calls instead of f(g(h()))
        public static TResult Map<T, TResult>(this T t, Func<T, TResult> f) => f(t);

        public static (bool exists, T property) TryGetProperty<T>(this object dynamic, string propertyName)
        {
            var property = dynamic.GetType().GetProperty(propertyName);
            return property == null
                ? default
                : (true, (T) property.GetValue(dynamic));
        }

        public static (bool exists, TResult property) Bind<T, TResult>(this (bool exists, T property) tuple, 
            Func<T, (bool exists, TResult property)> func)
        {
            return tuple.exists
                ? func(tuple.property)
                : default;
        }

        public static TResult? Bind<T, TResult>(this T? value, Func<T, TResult?> func) 
            where T : struct where TResult : struct
        {
            return value.HasValue
                ? func(value.Value)
                : null;
        }

        public static T? Return<T>(this T value) where T : struct => value;
    }
}
