using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Users
{
    [AllowAnonymous]
    public class LoginController : SurfaceController
    {
        private readonly IMemberService _memberService;
        private readonly IUserService _userService;

        public LoginController(IMemberService memberService, 
            IUserService userService)
        {
            _memberService = memberService;
            _userService = userService;
        }

        public ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View("~/App_Plugins/Users/Login/Login.cshtml", loginStatus);
        }

        [HttpPost]
        public ActionResult Login(string login, string password, string returnUrl)
        {
            var webSecurity = UmbracoContext.Security;

            var member = _memberService.GetByUsername(login);

            if (member == null)
            {
                if (webSecurity.ValidateBackOfficeCredentials(login, password))
                {
                    var user = _userService.GetByUsername(login);
                    member = _memberService.CreateMember(login, user.Email, user.Name, "Member"); //TODO:
                    member.SetValue("umbracoUserId", user.Id);
                    _memberService.Save(member);
                    _memberService.SavePassword(member, password);
                }
            }

            Members.Login(login, password);
            return Redirect(returnUrl ?? "/");
        }

        public void Logout()
        {
            Members.Logout();
        }
    }
}