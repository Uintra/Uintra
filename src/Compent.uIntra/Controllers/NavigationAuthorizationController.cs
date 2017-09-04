using uIntra.Core.User;
using uIntra.Navigation.Web;
using Umbraco.Core.Services;

namespace Compent.uIntra.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(IIntranetUserService<IIntranetUser> intranetUserService, IUserService userService) : 
            base(intranetUserService, userService)
        {
        }

    }
}