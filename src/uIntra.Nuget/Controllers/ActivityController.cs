using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Core.Web;

namespace uIntra.Controllers
{
    public class ActivityController: ActivityControllerBase
    {
        public ActivityController(IIntranetUserService<IIntranetUser> intranetUserService, IPermissionsService permissionsService) : base(intranetUserService, permissionsService)
        {
        }
    }
}