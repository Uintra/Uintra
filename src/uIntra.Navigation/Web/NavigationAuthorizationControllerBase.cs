using System.Web.Mvc;
using System.Web.Security;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.User;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace Uintra.Navigation.Web
{
    public abstract class NavigationAuthorizationControllerBase : SurfaceController
    {
        protected virtual string DefaultRedirectUrl { get; } = "/";
        protected virtual string UmbracoRedirectUrl { get; } = "/umbraco";
        protected virtual string LogoutViewPath { get; } = "~/App_Plugins/Users/Logout/Logout.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IUserService _userService;
        private readonly IApplicationSettings _applicationSettings;

        protected NavigationAuthorizationControllerBase(
            IIntranetUserService<IIntranetUser> intranetUserService,
            IUserService userService, IApplicationSettings applicationSettings)
        {
            _intranetUserService = intranetUserService;
            _userService = userService;
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult LoginToUmbraco()
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (!currentUser.UmbracoId.HasValue)
            {
                return Redirect(DefaultRedirectUrl);
            }

            var umbracoUser = _userService.GetUserById(currentUser.UmbracoId.Value);
            if (umbracoUser == null
                || umbracoUser.IsLockedOut
                || !umbracoUser.IsApproved)
            {
                return Redirect(DefaultRedirectUrl);
            }

            UmbracoContext.Security.PerformLogin(umbracoUser.Id);

            return Redirect(UmbracoRedirectUrl);
        }

        public virtual ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            if (_applicationSettings.GoogleOAuth.Enabled)
                return View(LogoutViewPath, new LogoutViewModel
                {
                    GoogleClientId = _applicationSettings.GoogleOAuth.ClientId,
                    LoginUrl = FormsAuthentication.LoginUrl
                });
            else return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
