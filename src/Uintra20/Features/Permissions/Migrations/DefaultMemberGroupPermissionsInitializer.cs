using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using static Uintra20.Infrastructure.Constants.UsersInstallationConstants;
using Resource = Uintra20.Features.Permissions.PermissionResourceTypeEnum;

namespace Uintra20.Features.Permissions.Migrations
{
    public class DefaultMemberGroupPermissionsInitializer
    {
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IPermissionsService _permissionsService;

        public DefaultMemberGroupPermissionsInitializer()
        {
            var dependencyResolver = DependencyResolver.Current;
            _intranetMemberGroupService = dependencyResolver.GetService<IIntranetMemberGroupService>();
            _permissionsService = dependencyResolver.GetService<IPermissionsService>();
        }

        public async Task SetupDefaultPermissions()
        {
            if (await _permissionsService.ContainsDefaultPermissionsAsync())
                return;
            
            var memberGroups = _intranetMemberGroupService.GetAll();

            var permissionsList = memberGroups
                .Where(n => n.Name == MemberGroups.GroupWebMaster
                            || n.Name == MemberGroups.GroupUiUser
                            || n.Name == MemberGroups.GroupUiPublisher)
                .SelectMany(member =>
                {
                    switch (member.Name)
                    {
                        case MemberGroups.GroupWebMaster:
                            return SetupWebMasterMemberGroup(member);
                        case MemberGroups.GroupUiPublisher:
                            return SetupUiPublisherMemberGroup(member);
                        default:
                            return SetupUiUserMemberGroup(member);
                    }
                })
                .ToArray();

            await _permissionsService.SaveAsync(permissionsList);
        }
        private static IEnumerable<PermissionUpdateModel> SetupWebMasterMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.DeleteOther);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Hide);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.HideOther);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.EditOwner);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.EditOther);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.EditOwner);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Hide);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.HideOther);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.EditOther);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiPublisherMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.Hide);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Groups, PermissionActionEnum.Hide);
        }

        private static IEnumerable<PermissionUpdateModel> SetupUiUserMemberGroup(IntranetMemberGroup group)
        {
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Create);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Edit);
            yield return CreatePermission(group, Resource.Social, PermissionActionEnum.Delete);
            yield return CreatePermission(group, Resource.Events, PermissionActionEnum.View);
            yield return CreatePermission(group, Resource.News, PermissionActionEnum.View);
        }

        private static PermissionUpdateModel CreatePermission(
            IntranetMemberGroup group,
            Resource resource,
            PermissionActionEnum action,
            bool allowed = true, bool enabled = true)
        {
            var identity = new PermissionSettingIdentity(action, resource);
            var settings = new PermissionSettingValues(allowed, enabled);

            return new PermissionUpdateModel(group, settings, identity);
        }
    }
}