using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Uintra20.Core.Extensions
{
    public static class EnumExtensions
    {

        public static int ToInt(this Enum enm) => (int)(object)enm;

        public static int? ToNullableInt(this Enum enm) => (int?)(object)enm;

        public static int? ToNullableInt(this Option<Enum> opt) => opt.Map(ToInt).ToNullable();

        public static T? ToEnum<T>(this int a)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), a))
            {
                return (T)Enum.Parse(typeof(T), a.ToString());
            }

            return default;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return Optional(enumValue).GetDisplayName();
        }

        public static string GetDisplayName(this Option<Enum> enumValue)
        {
            return enumValue.Map(i => Optional(i.GetAttribute<DisplayAttribute>())
                .Some(j => j.Name).None(i.ToString())).FirstOrDefault();
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