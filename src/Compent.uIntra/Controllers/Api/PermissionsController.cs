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
            IBasePermissionsService basePermissionsService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetActionTypeProvider intranetActionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService
            ) 
            : base(intranetMemberGroupProvider, basePermissionsService, activityTypeProvider, intranetActionTypeProvider, intranetMemberService)
        {
        }
    }
}