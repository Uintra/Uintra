using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Users
{
    [AllowAnonymous]
    public class LoginController : SurfaceController
    {
        private readonly IMemberService _memberService;

        public LoginController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View("~/App_Plugins/Users/Login/Login.cshtml", loginStatus);
        }

        [HttpPost]
        public ActionResult Login(string login, string password, string returnUrl)
        {
            var member = _memberService.GetByUsername(login);

            if (member == null)
            {
                return Redirect(HttpContext.Request.Url?.AbsoluteUri ?? "/");
            }

            Members.Login(login, password);
            return Redirect(returnUrl ?? "/");
        }

        public void Logout()
        {
            Members.Logout();
        }
    }
}