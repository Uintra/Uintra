using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;

namespace Compent.Uintra.Controllers.Api
{
    public class PermissionsController: PermissionsControllerBase
    {
        public PermissionsController(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsManagementService permissionsManagementService,
            IActivityTypeProvider activityTypeProvider,
            IIntranetActionTypeProvider intranetActionTypeProvider) 
            : base(intranetMemberGroupProvider, permissionsManagementService, activityTypeProvider, intranetActionTypeProvider)
        {
        }
    }
}