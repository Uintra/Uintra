using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;

namespace Compent.Uintra.Controllers.Api
{
    public class PermissionsController: PermissionsControllerBase
    {
        public PermissionsController(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService permissionsService,
            IPermissionResourceTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider actionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService
            ) 
            : base(intranetMemberGroupProvider, permissionsService, resourceTypeProvider, actionTypeProvider, intranetMemberService)
        {
        }
    }
}