using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uintra20.Attributes;
using Uintra20.Features.UserList.Models;

namespace Uintra20.Features.UserList.Helpers
{
    public static class UsersPresentationHelper
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

        public static bool RestrictAdminSelfDelete(
            MembersRowsViewModel rows,
            MemberModel member) =>
            rows.IsCurrentMemberGroupAdmin && rows.CurrentMember.Id != member.Member.Id;

        public static bool RestrictDeleteCreator(
            MembersRowsViewModel rows,
            MemberModel member) =>
            rows.IsCurrentMemberGroupAdmin && member.IsCreator;

        public static bool RestrictInvite(MembersRowsViewModel rows) =>
            rows.IsInvite;

        public static bool CanRenderToggleControl(
            MembersRowsViewModel rows,
            MemberModel member) =>
            RestrictAdminSelfDelete(rows, member)
            && !RestrictInvite(rows)
            && !RestrictDeleteCreator(rows, member);

        public static bool CanRenderDeleteControl(
            MembersRowsViewModel rows,
            MemberModel member) =>
            CanRenderToggleControl(rows, member);

        public static bool CanRenderInviteControl(
            MembersRowsViewModel rows,
            MemberModel member) =>
            RestrictAdminSelfDelete(rows, member)
            && RestrictInvite(rows);
    }
}