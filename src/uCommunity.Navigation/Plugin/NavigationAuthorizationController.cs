using System.Web.Mvc;
using System.Web.Security;
using uCommunity.Core.User;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Navigation.Plugin
{
    public class NavigationAuthorizationController : SurfaceController
    {
        private readonly IIntranetUserService _intranetUserService;
        private readonly IUserService _userService;

        public NavigationAuthorizationController(
            IIntranetUserService intranetUserService,
            IUserService userService)
        {
            _intranetUserService = intranetUserService;
            _userService = userService;
        }

        public ActionResult LoginToUmbraco()
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (!currentUser.UmbracoId.HasValue)
            {
                return Redirect("/");
            }

            var umbracoUser = _userService.GetUserById(currentUser.UmbracoId.Value);
            if (umbracoUser == null
                || umbracoUser.IsLockedOut
                || !umbracoUser.IsApproved)
            {
                return Redirect("/");
            }

            UmbracoContext.Security.PerformLogin(umbracoUser.Id);

            return Redirect("/umbraco");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
