using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Uintra.Core.Context.Extensions
{
    public class PropertyReflectionHelper
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> ReflectionCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetProperties(object instance)
        {
            if (instance is null) return new PropertyInfo[] {};
            var objectType = instance.GetType();
            if (!ReflectionCache.ContainsKey(objectType))
                ReflectionCache.TryAdd(objectType, ReflectProperties(instance).ToArray());

            return ReflectionCache[objectType];
        }

        public static IEnumerable<PropertyInfo> ReflectProperties(object instance)
        {
            var type = instance.GetType();

            var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => prop.GetIndexParameters().Length == 0 && prop.GetMethod != null);

            return propertyInfos;
        }
    }
}
