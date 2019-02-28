using Uintra.Core.ApplicationSettings;
using Uintra.Core.User;
using Uintra.Navigation.Web;

namespace Compent.Uintra.Controllers
{
    public class NavigationAuthorizationController : NavigationAuthorizationControllerBase
    {
        public NavigationAuthorizationController(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IApplicationSettings applicationSettings) : 
            base(intranetMemberService, applicationSettings)
        {
        }

    }
}