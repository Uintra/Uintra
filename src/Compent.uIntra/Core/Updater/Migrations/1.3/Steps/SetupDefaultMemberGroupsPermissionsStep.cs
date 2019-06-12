using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Persistence.Sql;
using Uintra.Core.Exceptions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Umbraco.Core;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.UsersInstallationConstants;
using static Uintra.Core.Permissions.PermissionActionEnum;
using Resource = Uintra.Core.Permissions.PermissionResourceTypeEnum;
using static LanguageExt.Prelude;
namespace Compent.Uintra.Core.Updater.Migrations._1._3.Steps
{
    public class SetupDefaultMemberGroupsPermissionsStep : IMigrationStep
    {
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IPermissionsService _permissionsService;

        public SetupDefaultMemberGroupsPermissionsStep(
            IIntranetMemberGroupService intranetMemberGroupService,
            IPermissionsService permissionsService)
        {
            _intranetMemberGroupService = intranetMemberGroupService;
            _permissionsService = permissionsService;
        }

        public ExecutionResult Execute()
        {
            new DbObjectContext().Database.Initialize(false);

            var memberGroups = _intranetMemberGroupService.GetAll();

            var permissions = memberGroups.Apply()  .Choose(group =>
            {
                switch (group.Name)
                {
                    case MemberGroups.GroupWebMaster:
                        return Some(SetupWebMasterMemberGroup(group));
                    case MemberGroups.GroupUiPublisher:
                        return Some(SetupUiPublisherMemberGroup(group));
                    case MemberGroups.GroupUiUser:
                        return Some(SetupUiUserMemberGroup(group));
                    default:
                        return None;
                }
            }).SelectMany(identity);

            _permissionsService.Save(permissions);
            return ExecutionResult.Success;
        }

        private static IEnumerable<PermissionUpdateModel> SetupWebMasterMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Bulletins, View);
            yield return CreatePermission(group, Resource.Bulletins, Create);
            yield return CreatePermission(group, Resource.Bulletins, Edit);
            yield return CreatePermission(group, Resource.Bulletins, Delete);
            yield return CreatePermission(group, Resource.Bulletins, EditOther);
            yield return CreatePermission(group, Resource.Bulletins, DeleteOther);
            yield return CreatePermission(group, Resource.Events, View);
            yield return CreatePermission(group, Resource.Events, Create);
            yield return CreatePermission(group, Resource.Events, Edit);
            yield return CreatePermission(group, Resource.Events, Hide);
            yield return CreatePermission(group, Resource.Events, HideOther);
            yield return CreatePermission(group, Resource.Events, EditOther);
            yield return CreatePermission(group, Resource.Events, EditOwner);
            yield return CreatePermission(group, Resource.News, View);
            yield return CreatePermission(group, Resource.News, Create);
            yield return CreatePermission(group, Resource.News, Edit);
            yield return CreatePermission(group, Resource.News, EditOther);
            yield return CreatePermission(group, Resource.News, EditOwner);
            yield return CreatePermission(group, Resource.Groups, Create);
            yield return CreatePermission(group, Resource.Groups, Edit);
            yield return CreatePermission(group, Resource.Groups, Hide);
            yield return CreatePermission(group, Resource.Groups, HideOther);
            yield return CreatePermission(group, Resource.Groups, EditOther);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiPublisherMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Bulletins, View);
            yield return CreatePermission(group, Resource.Bulletins, Create);
            yield return CreatePermission(group, Resource.Bulletins, Edit);
            yield return CreatePermission(group, Resource.Bulletins, Delete);
            yield return CreatePermission(group, Resource.Events, View);
            yield return CreatePermission(group, Resource.Events, Create);
            yield return CreatePermission(group, Resource.Events, Edit);
            yield return CreatePermission(group, Resource.Events, Hide);
            yield return CreatePermission(group, Resource.News, View);
            yield return CreatePermission(group, Resource.News, Create);
            yield return CreatePermission(group, Resource.News, Edit);
            yield return CreatePermission(group, Resource.Groups, Create);
            yield return CreatePermission(group, Resource.Groups, Edit);
            yield return CreatePermission(group, Resource.Groups, Hide);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiUserMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Bulletins, View);
            yield return CreatePermission(group, Resource.Bulletins, Create);
            yield return CreatePermission(group, Resource.Bulletins, Edit);
            yield return CreatePermission(group, Resource.Bulletins, Delete);
            yield return CreatePermission(group, Resource.Events, View);
            yield return CreatePermission(group, Resource.News, View);
        }

        private static PermissionUpdateModel CreatePermission(
            IntranetMemberGroup group,
            Resource resource,
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