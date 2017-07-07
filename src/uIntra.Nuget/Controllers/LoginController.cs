using uIntra.Core;
using uIntra.Users.Web;

namespace Compent.uIntra.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class LoginController : LoginControllerBase
    {
        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(ITimezoneOffsetProvider timezoneOffsetProvider) :
            base(timezoneOffsetProvider)
        {
        }
    }
}