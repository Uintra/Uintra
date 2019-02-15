using System;
using System.Collections.Generic;
using Uintra.Core.Permissions.Sql;

namespace Uintra.Core.Permissions.Models
{
    public class BasePermissionModel
    {
        public Guid Id { get; }
        public IntranetMemberGroup Group { get; }
        public Enum ActionType { get; }
        public bool IsAllowed { get; }
        public bool IsEnabled { get; }

        private BasePermissionModel(Guid id, IntranetMemberGroup group, Enum permissionType, bool isAllowed, bool isEnabled)
        {
            Id = id;
            Group = group;
            ActionType = permissionType;
            IsAllowed = isAllowed;
            IsEnabled = isEnabled;
        }


        public static BasePermissionModel Of(IDictionary<int,IntranetMemberGroup> groupDictionary, IDictionary<int, Enum> actionDictionary, PermissionEntity entity) =>
            new BasePermissionModel(
                entity.Id,
                groupDictionary[entity.IntranetMemberGroupId],
                actionDictionary[entity.IntranetActionId],
                entity.IsAllowed,
                entity.IsEnabled);

        public static BasePermissionModel Of(PermissionManagementModel permissionManagementModel) =>
            new BasePermissionModel(
                Guid.NewGuid(),
                permissionManagementModel.Group,
                permissionManagementModel.SettingIdentity.ActionType,
                permissionManagementModel.SettingValues.IsAllowed,
                permissionManagementModel.SettingValues.IsEnabled);

    }
}