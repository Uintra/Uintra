using uCommunity.Core.User;
using uCommunity.Navigation.Web;
using uCommunity.Users.Core;
using Umbraco.Core.Services;

namespace Compent.uIntra.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(IIntranetUserService<IntranetUser> intranetUserService, IUserService userService) : 
            base(intranetUserService, userService)
        {
        }

    }
}