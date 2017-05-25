using System.Web.Mvc;
using uCommunity.Users.Core;
using uIntra.Core;
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
        private readonly ITimezoneOffsetProvider _timezoneOffsetProvider;

        protected LoginControllerBase(IMemberService memberService, ITimezoneOffsetProvider timezoneOffsetProvider)
        {
            _memberService = memberService;
            _timezoneOffsetProvider = timezoneOffsetProvider;
        }

        public virtual ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View(LoginViewPath, loginStatus);
        }

        [HttpPost]
        public virtual ActionResult Login(LoginModelBase model)
        {
            var redirectUrl = model.ReturnUrl ?? DefaultRedirectUrl;
            if (Members.Login(model.Login, model.Password))
            {
                _timezoneOffsetProvider.SetTimezoneOffset(model.ClientTimezoneOffset);
            }
            return Redirect(redirectUrl);

        }

        public virtual void Logout()
        {
            Members.Logout();
        }
    }
}