using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Uintra.Users.Web
{
    public abstract class UserListControllerBase : SurfaceController
    {
        protected virtual string ViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";

        public UserListControllerBase()
        {

        }

        public ActionResult Render(UserListModel model)
        {

            return View(ViewPath, model);
        }
    }
}
