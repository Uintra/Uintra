using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra.Core.User;
using Umbraco.Web.Mvc;

namespace Uintra.Users.Web
{
    public abstract class UserListControllerBase : SurfaceController
    {
        protected virtual string ViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public UserListControllerBase(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public ActionResult Render(UserListModel model)
        {
            var viewModel = new UserListViewModel()
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title
            };
            return View(ViewPath, viewModel);
        }
    }
}
