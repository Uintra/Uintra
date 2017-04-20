using System;
using uCommunity.Core.User;

namespace uCommunity.Core.Extentions
{
    public static class EnumExtentions
    {
        public static string ToRoleName(this IntranetRolesEnum enm)
        {
            return enm.ToString();
        }

        public static string ToRoleAlias(this IntranetRolesEnum enm)
        {
            return enm.ToString().ToLower();
        }

        public static T? ToEnum<T>(this int a)
            where T : struct
        {
            if (Enum.IsDefined(typeof(T), a))
            {
                return (T)Enum.Parse(typeof(T), a.ToString());
            }

            return default(T?);
        }
    }
}
