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
        private readonly IOldPermissionsService _oldPermissionsService;

        public GroupPermissionsService(IOldPermissionsService oldPermissionsService)
        {
            _oldPermissionsService = oldPermissionsService;
        }

        public bool HasPermission(IRole role, IntranetActionEnum action)
        {
            var hasPermission = _oldPermissionsService.IsRoleHasPermissions(role, $"Group{action}");
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content, IRole role)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _oldPermissionsService.IsRoleHasPermissions(role, CreateGroupPermissionName);
                return hasPermission;
            }
            return true;
        }
    }
}
