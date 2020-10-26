using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using LightInject;
using UBaseline.Core.Extensions;
using Uintra20.Core.Updater;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;

namespace Uintra20.Core.Updater._2._0.Steps
{
    public class SetupDefaultMemberGroupsPermissionsStep : IMigrationStep
    {
        private readonly ILogger _logger;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IPermissionsService _permissionsService;

        public SetupDefaultMemberGroupsPermissionsStep()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _intranetMemberGroupService = Current.Factory.EnsureScope(s=>s.GetInstance<IIntranetMemberGroupService>());
            _permissionsService =  Current.Factory.EnsureScope(s=>s.GetInstance<IPermissionsService>());
        }

        public ExecutionResult Execute()
        {
            _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("SetupDefaultMemberGroupsPermissionsStep is running");
            var existedPermissions = _permissionsService.GetAll();
            if (existedPermissions.Any())
            {
                _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("Database contains some permissions. Step has been skipped");
                return ExecutionResult.Success;
            }
            
            var memberGroups = _intranetMemberGroupService.GetAll();

            var permissions = memberGroups.Select(group =>
                {
                    switch (group.Name)
                    {
                        case UsersInstallationConstants.MemberGroups.GroupWebMaster:
                            _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("Setting permissions for Web Master...");
                            return SetupWebMasterMemberGroup(group);
                        case UsersInstallationConstants.MemberGroups.GroupUiPublisher:
                            _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("Setting permissions for UI Publisher...");
                            return SetupUiPublisherMemberGroup(group);
                        case UsersInstallationConstants.MemberGroups.GroupUiUser:
                            _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("Setting permissions for UI User...");
                            return SetupUiUserMemberGroup(group);
                        default:
                            return null;
                    }
                })
                .WhereNotNull()
                .SelectMany(p => p);

            _logger.Info<SetupDefaultMemberGroupsPermissionsStep>($"{permissions.Count()} permissions has been saved");
            _permissionsService.Save(permissions);
            MemoryCache.Default.Trim(100);
            return ExecutionResult.Success;
        }

        private static IEnumerable<PermissionUpdateModel> SetupWebMasterMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.DeleteOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Hide);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.HideOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.EditOwner);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.EditOwner);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Hide);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.HideOther);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.EditOther);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiPublisherMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.Hide);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Groups, PermissionActionEnum.Hide);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiUserMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, PermissionResourceTypeEnum.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, PermissionResourceTypeEnum.News, PermissionActionEnum.View);
        }

        private static PermissionUpdateModel CreatePermission(
            IntranetMemberGroup group,
            PermissionResourceTypeEnum resource,
            PermissionActionEnum action,
            bool allowed = true, bool enabled = true)
        {
            var identity = new PermissionSettingIdentity(action, resource);
            var settings = new PermissionSettingValues(allowed, enabled);
            return new PermissionUpdateModel(group, settings, identity);
        }

        public void Undo()
        {
        }
    }
}