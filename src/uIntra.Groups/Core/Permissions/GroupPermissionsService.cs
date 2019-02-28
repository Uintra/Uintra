using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public class GroupPermissionsService : IGroupPermissionsService
    {        
        protected const string GroupsCreatePage = "groupsCreatePage";
        private readonly IPermissionsService _basePermissionsService;

        public GroupPermissionsService(IPermissionsService basePermissionsService)
        {
            _basePermissionsService = basePermissionsService;
        }

        public bool HasPermission(PermissionSettingIdentity permissionSettingIdentity)
        {
            var hasPermission = _basePermissionsService.Check(permissionSettingIdentity);
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _basePermissionsService.Check(
                    PermissionSettingIdentity.Of(PermissionResourceTypeEnum.Groups, PermissionActionEnum.Create));

                return hasPermission;
            }
            return true;
        }
    }
}
