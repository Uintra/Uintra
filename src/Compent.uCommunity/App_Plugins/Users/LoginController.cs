using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uCommunity.Users
{
    [AllowAnonymous]
    public class LoginController : SurfaceController
    {
        public ActionResult Login()
        {
            var loginStatus = Members.GetCurrentLoginStatus();
            return View("~/App_Plugins/Users/Login/Login.cshtml", loginStatus);
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            var membersService = ApplicationContext.Services.MemberService;
            var usersService = ApplicationContext.Services.UserService;
            var webSrcurity = UmbracoContext.Security;

            var member = membersService.GetByUsername(login);

            if (member == null)
            {
                if (webSrcurity.ValidateBackOfficeCredentials(login, password))
                {
                    var user = usersService.GetByUsername(login);
                    member = membersService.CreateMember(login, user.Email, user.Name, "Member"); //TODO:
                    member.SetValue("umbracoUserId", user.Id);
                    membersService.Save(member);
                    membersService.SavePassword(member, password);
                }
            }

            Members.Login(login, password);
            return Redirect("/");
        }
    }
}