﻿using System.Web.Mvc;
using System.Web.Security;
using Uintra.Core;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Localization;
using Umbraco.Web.Mvc;

namespace Uintra.Users.Web
{
    [AllowAnonymous]
    public abstract class LoginControllerBase : SurfaceController
    {
        protected virtual string LoginViewPath { get; } = "~/App_Plugins/Users/Login/Login.cshtml";
        protected virtual string DefaultRedirectUrl { get; } = "/";

        private readonly IClientTimezoneProvider _clientTimezoneProvider;
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly IApplicationSettings _applicationSettings;

        protected LoginControllerBase(
            IClientTimezoneProvider clientTimezoneProvider,
            IIntranetLocalizationService intranetLocalizationService,
            IApplicationSettings applicationSettings)
        {
            _clientTimezoneProvider = clientTimezoneProvider;
            _intranetLocalizationService = intranetLocalizationService;
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult Login()
        {
            if (Members.GetCurrentLoginStatus().IsLoggedIn)
            {
                return Redirect(DefaultRedirectUrl);
            }
            
            return View(LoginViewPath, new LoginModelBase());
        }

        [HttpPost]
        public virtual ActionResult Login(LoginModelBase model)
        {            
            if (!ModelState.IsValid)
            {
                return View(LoginViewPath, model);
            }

            if (!Membership.ValidateUser(model.Login, model.Password))
            {
                ModelState.AddModelError("UserValidation",
                    _intranetLocalizationService.Translate("Login.Validation.UserNotValid"));
                return View(LoginViewPath, model);
            }

            var redirectUrl = model.ReturnUrl ?? DefaultRedirectUrl;

            if (Members.Login(model.Login, model.Password))
            {
                _clientTimezoneProvider.SetClientTimezone(model.ClientTimezoneId);
            }
            return Redirect(redirectUrl);
        }

        public virtual void Logout()
        {
            Members.Logout();
        }

        protected virtual GoogleAuthenticationSettings GetGoogleSettings()
        {
            return new GoogleAuthenticationSettings()
            {
                AuthenticationEnabled = _applicationSettings.GoogleOAuth.Enabled,
                ClientId = _applicationSettings.GoogleOAuth.ClientId,
                Domain = _applicationSettings.GoogleOAuth.Domain
            };
        }
    }
}