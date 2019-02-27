using Uintra.Core.Permissions;
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

        public bool HasPermission(PermissionActionEnum action, PermissionActivityTypeEnum activityType)
        {
            var hasPermission = _permissionsService.Check(activityType, action);
            return hasPermission;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _permissionsService.Check(PermissionActivityTypeEnum.Groups, PermissionActionEnum.Create);
                return hasPermission;
            }
            return true;
        }
    }
}
