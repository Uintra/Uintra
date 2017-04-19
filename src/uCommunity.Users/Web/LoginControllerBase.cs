using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Users.Web
{
    [AllowAnonymous]
    public abstract class LoginControllerBase : SurfaceController
    {
        protected readonly IMemberService _memberService;
        protected virtual string LoginView => "~/App_Plugins/Users/Login/Login.cshtml";


        protected LoginControllerBase(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public virtual ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View(LoginView, loginStatus);
        }

        [HttpPost]
        public virtual ActionResult Login(string login, string password, string returnUrl)
        {
            var redirectUrl = returnUrl ?? "/";
            if (!Members.Login(login, password))
            {
                redirectUrl = HttpContext.Request.Url?.AbsoluteUri ?? "/";

            }
            return Redirect(redirectUrl);

        }

        public virtual void Logout()
        {
            Members.Logout();
        }
    }
}