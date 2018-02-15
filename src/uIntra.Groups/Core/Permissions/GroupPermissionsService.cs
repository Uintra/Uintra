using Uintra.Core.Activity;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public class GroupPermissionsService : IGroupPermissionsService
    {
        protected const string CreateGroupPermissionName = "GroupCreate";
        protected const string GroupsCreatePage = "groupsCreatePage";
        private readonly IPermissionsService _permissionsService;

        public GroupPermissionsService(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        public bool HasPermission(IRole role, IntranetActivityActionEnum action)
        {
            var hasPermission = _permissionsService.IsRoleHasPermissions(role, $"Group{action}");
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content, IRole role)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _permissionsService.IsRoleHasPermissions(role, CreateGroupPermissionName);
                return hasPermission;
            }
            return true;
        }
    }
}
