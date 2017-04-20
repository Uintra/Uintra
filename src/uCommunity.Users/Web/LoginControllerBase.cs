using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Users.Web
{
    [AllowAnonymous]
    public abstract class LoginControllerBase : SurfaceController
    {
        protected virtual string LoginViewPath { get; } = "~/App_Plugins/Users/Login/Login.cshtml";
        protected virtual string DefaultRedirectUrl { get; } = "/";

        protected readonly IMemberService _memberService;

        protected LoginControllerBase(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public virtual ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View(LoginViewPath, loginStatus);
        }

        [HttpPost]
        public virtual ActionResult Login(string login, string password, string returnUrl)
        {
            var redirectUrl = returnUrl ?? DefaultRedirectUrl;
            if (!Members.Login(login, password))
            {
                redirectUrl = HttpContext.Request.Url?.AbsoluteUri ?? DefaultRedirectUrl;

            }
            return Redirect(redirectUrl);

        }

        public virtual void Logout()
        {
            Members.Logout();
        }
    }
}