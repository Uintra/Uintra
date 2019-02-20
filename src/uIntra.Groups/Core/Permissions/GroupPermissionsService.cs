using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Umbraco.Core.Models;

namespace Uintra.Groups.Permissions
{
    public class GroupPermissionsService : IGroupPermissionsService
    {        
        protected const string GroupsCreatePage = "groupsCreatePage";
        private readonly IBasePermissionsService _basePermissionsService;

        public GroupPermissionsService(IBasePermissionsService basePermissionsService)
        {
            _basePermissionsService = basePermissionsService;
        }

        public bool HasPermission(PermissionActionEnum action, PermissionActivityTypeEnum activityType)
        {
            var hasPermission = _basePermissionsService.Check(activityType, action);
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _basePermissionsService.Check(PermissionActivityTypeEnum.Group, PermissionActionEnum.Create);
                return hasPermission;
            }
            return true;
        }
    }
}
