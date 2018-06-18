using System;
using System.Linq;
using System.Reflection;

namespace Uintra.Core.Extensions
{
    public static class EnumExtensions
    {

        public static int ToInt(this Enum enm) => (int) (object) enm;

        public static T? ToEnum<T>(this int a)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), a))
            {
                return (T) Enum.Parse(typeof(T), a.ToString());
            }

            return default;
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }
}
