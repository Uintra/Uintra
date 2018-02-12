using System.Web.Mvc;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.Localization;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers
{
    [AllowAnonymous]
    [ThreadCulture]
    public class LoginController : LoginControllerBase
    {
        protected override string LoginViewPath => "~/Views/Login/Login.cshtml";

        public LoginController(ITimezoneOffsetProvider timezoneOffsetProvider, IIntranetLocalizationService  intranetLocalizationService) :
            base(timezoneOffsetProvider, intranetLocalizationService)
        {
        }
    }
}