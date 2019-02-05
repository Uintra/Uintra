using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
{
    public class ActivityController: ActivityControllerBase
    {
        protected override string ItemHeaderViewPath { get; } = "~/Views/Activity/ItemHeader.cshtml";

        public ActivityController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IOldPermissionsService oldPermissionsService,
            IActivityTypeProvider activityTypeProvider) 
            : base(intranetMemberService, oldPermissionsService)
        {
        }
    }
}