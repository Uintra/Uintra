using uCommunity.Core.User;
using uCommunity.Navigation.Web;
using Umbraco.Core.Services;

namespace Compent.uCommunity.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(IIntranetUserService intranetUserService, IUserService userService) : 
            base(intranetUserService, userService)
        {
        }

    }
}