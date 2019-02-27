using Uintra.Core.Permissions;
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
            IActivityTypeProvider activityTypeProvider,
            IPermissionActionTypeProvider intranetActionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService
            ) 
            : base(intranetMemberGroupProvider, permissionsService, activityTypeProvider, intranetActionTypeProvider, intranetMemberService)
        {
        }
    }
}