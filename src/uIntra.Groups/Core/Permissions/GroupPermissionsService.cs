using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups.Installer;
using Umbraco.Core.Models;

namespace uIntra.Groups.Permissions
{
    public class GroupPermissionsService : IGroupPermissionsService
    {
        protected const string CreateGroupPermissionName = "GroupCreate";
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
            if (content.DocumentTypeAlias == GroupsInstallationConstants.DocumentTypeAliases.GroupsCreatePage)
            {
                var hasPermission = _permissionsService.IsRoleHasPermissions(role, CreateGroupPermissionName);
                return hasPermission;
            }
            return true;
        }
    }
}
