using uCommunity.Core;
using uCommunity.Users.Web;
using Umbraco.Core.Services;

namespace Compent.uIntra.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class LoginController : LoginControllerBase
    {
        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(IMemberService memberService, ITimezoneOffsetProvider timezoneOffsetProvider) :
            base(memberService, timezoneOffsetProvider)
        {
        }
    }
}