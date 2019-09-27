using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uintra.Users.Attributes;
using Uintra.Users.UserList;

namespace Uintra.Users.Helpers
{
    public static class UserListPresentationHelper
    {
        public static IEnumerable<ProfileColumnModel> AddManagementColumn(IEnumerable<ProfileColumnModel> columns) =>
            columns.Append(new ProfileColumnModel
            {
                Alias = "Management",
                Id = 100,
                Name = "Management",
                PropertyName = "management",
                SupportSorting = false,
                Type = ColumnType.GroupManagement
            });

        public static IEnumerable<ProfileColumnModel> ExtendIfGroupMembersPage(
            Guid? groupId, 
            IEnumerable<ProfileColumnModel> columns)
        {
            if (!groupId.HasValue) return columns;

            return columns.Append(new ProfileColumnModel
            {
                Alias = "Group",
                Id = 99,
                Name = "Group",
                PropertyName = "role",
                SupportSorting = false,
                Type = ColumnType.GroupRole
            }).Append(new ProfileColumnModel
            {
                Alias = "Management",
                Id = 100,
                Name = "Management",
                PropertyName = "management",
                SupportSorting = false,
                Type = ColumnType.GroupManagement
            });
        }

        public static IEnumerable<ProfileColumnModel> GetProfileColumns()
        {
            var columns = typeof(MemberModel).GetCustomAttributes<UIColumnAttribute>().Select(i => new ProfileColumnModel()
            {
	            Id = i.Id,
	            Name = i.DisplayName,
	            Type = i.Type,
	            Alias = i.Alias,
	            PropertyName = i.PropertyName,
	            SupportSorting = i.SupportSorting
            }).OrderBy(i => i.Id);
            return columns;
        }
    }
}