using System.Collections.Generic;
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
            var columns = typeof(MemberModel).GetCustomAttributes<UIColumnAttribute>().Select(i =>
            {
                return new ProfileColumnModel()
                {
                    Id = i.Id,
                    Name = i.DisplayName,
                    Type = i.Type,
                    Alias = i.Alias,
                    PropertyName = i.PropertyName,
                    SupportSorting = i.SupportSorting
                };
            }).OrderBy(i => i.Id);
            return columns;
        }
    }
}
