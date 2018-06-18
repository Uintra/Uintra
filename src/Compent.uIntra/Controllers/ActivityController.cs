using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers
{
    public class ActivityController: ActivityControllerBase
    {
        protected override string ItemHeaderViewPath { get; } = "~/Views/Activity/ItemHeader.cshtml";

        public ActivityController(IIntranetUserService<IIntranetUser> intranetUserService,
            IPermissionsService permissionsService,
            ActivityTypeProvider activityTypeProvider) 
            : base(intranetUserService, permissionsService, activityTypeProvider)
        {
        }
    }
}