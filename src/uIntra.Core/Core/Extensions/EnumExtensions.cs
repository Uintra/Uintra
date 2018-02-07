using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uIntra.Core.User;

namespace uIntra.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToRoleName(this IntranetRolesEnum enm)
        {
            return enm.ToString();
        }

        public static string ToRoleAlias(this IntranetRolesEnum enm)
        {
            return enm.ToString().ToLower();
        }

        public static int ToInt(this Enum enm) => (int) (object) enm;

        public static T? ToEnum<T>(this int a)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), a))
            {
                return (T) Enum.Parse(typeof(T), a.ToString());
            }

            return default(T?);
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static IEnumerable<T> GetEnumCases<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }


    }
}
