using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public class GroupPermissionsService : IGroupPermissionsService
    {        
        protected const string GroupsCreatePage = "groupsCreatePage";
        private readonly IPermissionsService _permissionsService;

        public GroupPermissionsService(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        public bool HasPermission(PermissionSettingIdentity permissionSettingIdentity)
        {
            var hasPermission = _permissionsService.Check(permissionSettingIdentity);
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _permissionsService.Check(PermissionResourceTypeEnum.Groups, PermissionActionEnum.Create);

                return hasPermission;
            }

            return true;
        }
    }
}
