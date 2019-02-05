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

        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;        
        private readonly IApplicationSettings _applicationSettings;

        protected NavigationAuthorizationControllerBase(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IUserService userService, IApplicationSettings applicationSettings)
        {
            _intranetMemberService = intranetMemberService;            
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            if (currentMember.RelatedUser == null)
            {
                return Redirect(DefaultRedirectUrl);
            }


            if (currentMember.RelatedUser.IsLockedOut || !currentMember.RelatedUser.IsApproved)
            {
                return Redirect(DefaultRedirectUrl);
            }

            UmbracoContext.Security.PerformLogin(currentMember.RelatedUser.Id);

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
            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
