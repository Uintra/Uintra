using System;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Users.Web;
using Umbraco.Core.Services;

namespace Compent.uCommunity.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class LoginController: LoginControllerBase
    {
        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";
        private readonly ITimezoneOffsetProvider _timezoneOffsetProvider;

        public LoginController(IMemberService memberService, ITimezoneOffsetProvider timezoneOffsetProvider) : base(memberService)
        {
            _timezoneOffsetProvider = timezoneOffsetProvider;
        }

        [HttpPost]
        public override ActionResult Login(string login, string password, string returnUrl)
        {
            var redirectUrl = returnUrl ?? DefaultRedirectUrl;
            if (Members.Login(login, password))
            {
                redirectUrl = HttpContext.Request.Url?.AbsoluteUri ?? DefaultRedirectUrl;
                _timezoneOffsetProvider.SetTimezoneOffset(60);
                var check = _timezoneOffsetProvider.GetTimezoneOffset();

                var serverUtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Minutes;
            }
            return Redirect(redirectUrl);

        }
    }
}