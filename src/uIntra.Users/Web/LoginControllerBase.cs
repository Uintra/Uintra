﻿using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Web.Mvc;

namespace uIntra.Users.Web
{
    [AllowAnonymous]
    public abstract class LoginControllerBase : SurfaceController
    {
        protected virtual string LoginViewPath { get; } = "~/App_Plugins/Users/Login/Login.cshtml";
        protected virtual string DefaultRedirectUrl { get; } = "/";

        private readonly ITimezoneOffsetProvider _timezoneOffsetProvider;

        protected LoginControllerBase(ITimezoneOffsetProvider timezoneOffsetProvider)
        {
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

            if (model.Login.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
            {
                return Redirect(redirectUrl);
            }

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