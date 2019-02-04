using Uintra.Core.ApplicationSettings;
using Uintra.Core.User;
using Uintra.Navigation.Web;
using Umbraco.Core.Services;

namespace Compent.Uintra.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IUserService userService, IApplicationSettings applicationSettings) : 
            base(intranetMemberService, userService, applicationSettings)
        {
        }

    }
}