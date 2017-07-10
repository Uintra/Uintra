using uIntra.Core.User;
using uIntra.Navigation.Web;
using uIntra.Users;
using Umbraco.Core.Services;

namespace uIntra.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(IIntranetUserService<IntranetUser> intranetUserService, IUserService userService) : 
            base(intranetUserService, userService)
        {
        }

    }
}