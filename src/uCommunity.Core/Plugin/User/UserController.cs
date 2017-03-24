using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uCommunity.Core.User
{
    public class UserController : SurfaceController
    {
        public PartialViewResult UserView(IIntranetUser user)
        {
            return PartialView("~/App_Plugins/Core/User/UserView.cshtml", user);
        }
    }
}
