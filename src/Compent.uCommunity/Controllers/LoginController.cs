using System.Web.Mvc;
using uCommunity.Users.Web;
using Umbraco.Core.Services;

namespace Compent.uCommunity.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class LoginController: LoginControllerBase
    {
        protected override string LoginView => "~/Views/Login/Login.cshtml";

        public LoginController(IMemberService memberService) : base(memberService)
        {
        }
    }
}