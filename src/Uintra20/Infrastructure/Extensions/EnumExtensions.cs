using Compent.Shared.Extensions.Bcl;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Uintra20.Infrastructure.Extensions
{
    public static class EnumExtensions
    {

        public static int ToInt(this Enum enm) => (int)(object)enm;

        public static int? ToNullableInt(this Enum enm) => enm?.ToInt();
        
        public static T? ToEnum<T>(this int a)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), a))
            {
                return (T)Enum.Parse(typeof(T), a.ToString());
            }

            return null;
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var name = enumValue.GetAttribute<DisplayAttribute>()?.Name;

            return name.HasValue() ? name : enumValue.ToString();
        }
    }
}