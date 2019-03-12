using System.Web.Mvc;
using System.Web.Security;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.User;
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
            IApplicationSettings applicationSettings)
        {
            _intranetMemberService = intranetMemberService;            
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult LoginToUmbraco()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var relatedUser = currentMember.RelatedUser
                .Filter(user => user.IsValid);

            return relatedUser
                .Match(
                    Some: user =>
                    {
                        UmbracoContext.Security.PerformLogin(user.Id);
                        return Redirect(UmbracoRedirectUrl);
                    },
                    None: () => Redirect(DefaultRedirectUrl));
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
