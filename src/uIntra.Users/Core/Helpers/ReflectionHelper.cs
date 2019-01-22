﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uintra.Users.Attributes;
using Uintra.Users.UserList;
using Umbraco.Core;

namespace Uintra.Users.Helpers
{
    public static class ReflectionHelper
    {
        private static PropertyInfo[] Properties { get; set; }

        static ReflectionHelper()
        {
            Properties = typeof(MemberModel).GetPublicProperties();
        }

        public static object GetUIColumnValue(MemberModel obj, ProfileColumnModel column)
        {
            var prop = Properties.FirstOrDefault(i =>
                {
                    var attr = i.FirstAttribute<UIColumnAttribute>();
                    return attr != null && attr.Id == column.Id;
                });
            return prop?.GetValue(obj);
        }

        public static Dictionary<string, string> GetUserListTranslations(string keyFormat)
        {
            var result = new Dictionary<string, string>();
            foreach (var prop in Properties)
            {
                var attr = prop.FirstAttribute<UIColumnAttribute>();
                if (attr == null)
                    continue;
                result.Add(string.Format(keyFormat, attr.Alias), attr.DisplayName);
            }
            return result;
        }

        public static IEnumerable<ProfileColumnModel> GetProfileColumns()
        {
            var columns = Properties.Select(i =>
                {
                    var attr = i.FirstAttribute<UIColumnAttribute>();
                    if (attr == null)
                        return null;
                    return new ProfileColumnModel()
                    {
                        Id = attr.Id,
                        Name = attr.DisplayName,
                        Type = attr.Type,
                        Alias = attr.Alias,
                        PropertyName = attr.PropertyName,
                        SupportSorting = attr.SupportSorting
                    };
                }).WhereNotNull().OrderBy(i => i.Id);
            return columns;
        }
    }
}
