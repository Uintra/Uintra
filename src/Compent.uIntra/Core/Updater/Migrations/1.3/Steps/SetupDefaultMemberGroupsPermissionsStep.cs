using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Exceptions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Umbraco.Core;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.UsersInstallationConstants;
using static Uintra.Core.Permissions.PermissionActionEnum;
using Resource = Uintra.Core.Permissions.PermissionResourceTypeEnum;

namespace Compent.Uintra.Core.Updater.Migrations._1._3.Steps
{
    public class SetupDefaultMemberGroupsPermissionsStep : IMigrationStep
    {
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IPermissionsService _permissionsService;
        private readonly IExceptionLogger _exceptionLogger;

        public SetupDefaultMemberGroupsPermissionsStep(
            IIntranetMemberGroupService intranetMemberGroupService,
            IPermissionsService permissionsService,
            IExceptionLogger exceptionLogger)
        {
            _intranetMemberGroupService = intranetMemberGroupService;
            _permissionsService = permissionsService;
            _exceptionLogger = exceptionLogger;
        }

        public ExecutionResult Execute()
        {
            var memberGroups = _intranetMemberGroupService.GetAll().ToArray();

            throw new Exception(memberGroups.Aggregate(string.Empty, (acc, e) => acc + $"{e.Id}+{e.Name}/"));


            var permissions = new List<PermissionUpdateModel>();
            foreach (var group in memberGroups)
            {

                switch (group.Name)
                {
                    case MemberGroups.GroupWebMaster: permissions.AddRange(SetupWebMasterMemberGroup(group)); break;
                    case MemberGroups.GroupUiPublisher: permissions.AddRange(SetupUiPublisherMemberGroup(group)); break;
                    case MemberGroups.GroupUiUser: permissions.AddRange(SetupUiUserMemberGroup(group)); break; 
                    default:
                        break;
                }
            }
            _permissionsService.Save(permissions);
            return ExecutionResult.Success;
        }

        private IEnumerable<PermissionUpdateModel> SetupWebMasterMemberGroup(IntranetMemberGroup group)
        {
            var groupPermissions = new List<PermissionUpdateModel>();
            groupPermissions.AddRange(new[]
            {
                CreatePermission(group, Resource.Bulletins, View),
                CreatePermission(group, Resource.Bulletins, Create),
                CreatePermission(group, Resource.Bulletins, Edit),
                CreatePermission(group, Resource.Bulletins, Delete),
                CreatePermission(group, Resource.Bulletins, EditOther),
                CreatePermission(group, Resource.Bulletins, DeleteOther),
                CreatePermission(group, Resource.Events, View),
                CreatePermission(group, Resource.Events, Create),
                CreatePermission(group, Resource.Events, Edit),
                CreatePermission(group, Resource.Events, Hide),
                CreatePermission(group, Resource.Events, HideOther),
                CreatePermission(group, Resource.Events, EditOther),
                CreatePermission(group, Resource.Events, EditOwner),
                CreatePermission(group, Resource.News, View),
                CreatePermission(group, Resource.News, Create),
                CreatePermission(group, Resource.News, Edit),
                CreatePermission(group, Resource.News, EditOther),
                CreatePermission(group, Resource.News, EditOwner),
                CreatePermission(group, Resource.Groups, Create),
                CreatePermission(group, Resource.Groups, Edit),
                CreatePermission(group, Resource.Groups, Hide),
                CreatePermission(group, Resource.Groups, HideOther),
                CreatePermission(group, Resource.Groups, EditOther)
            });
            return groupPermissions;
        }

        private IEnumerable<PermissionUpdateModel> SetupUiPublisherMemberGroup(IntranetMemberGroup group)
        {
            var groupPermissions = new List<PermissionUpdateModel>();
            groupPermissions.AddRange(new[]
            {
                CreatePermission(group, Resource.Bulletins, View),
                CreatePermission(group, Resource.Bulletins, Create),
                CreatePermission(group, Resource.Bulletins, Edit),
                CreatePermission(group, Resource.Bulletins, Delete),
                CreatePermission(group, Resource.Events, View),
                CreatePermission(group, Resource.Events, Create),
                CreatePermission(group, Resource.Events, Edit),
                CreatePermission(group, Resource.Events, Hide),
                CreatePermission(group, Resource.News, View),
                CreatePermission(group, Resource.News, Create),
                CreatePermission(group, Resource.News, Edit),
                CreatePermission(group, Resource.Groups, Create),
                CreatePermission(group, Resource.Groups, Edit),
                CreatePermission(group, Resource.Groups, Hide)
            });
            return groupPermissions;
        }

        private IEnumerable<PermissionUpdateModel> SetupUiUserMemberGroup(IntranetMemberGroup group)
        {
            var groupPermissions = new List<PermissionUpdateModel>();
            groupPermissions.AddRange(new[]
            {
                CreatePermission(group, Resource.Bulletins, View),
                CreatePermission(group, Resource.Bulletins, Create),
                CreatePermission(group, Resource.Bulletins, Edit),
                CreatePermission(group, Resource.Bulletins, Delete),
                CreatePermission(group, Resource.Events, View),
                CreatePermission(group, Resource.News, View)
            });
            return groupPermissions;
        }

        private PermissionUpdateModel CreatePermission(IntranetMemberGroup group, 
            PermissionResourceTypeEnum resource,
            PermissionActionEnum action,
            bool allowed = true, bool enabled = true)
        {
            var identity = PermissionSettingIdentity.Of(action, resource);
            var settings = PermissionSettingValues.Of(allowed, enabled);
            return PermissionUpdateModel.Of(group, settings, identity);
        }

        public void Undo()
        {
            
        }
    }
}