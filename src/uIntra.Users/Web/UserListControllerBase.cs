using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Web.Mvc;
using Newtonsoft.Json;

namespace Uintra.Users.Web
{
    public abstract class UserListControllerBase : SurfaceController
    {
        protected virtual string UserListViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";
        protected virtual string UsersRowsViewPath => @"~/App_Plugins/Users/UserList/UsersRowsView.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public UserListControllerBase(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public virtual ActionResult Render(UserListModel model)
        {
            string json = (string)model.SelectedProperties.ToString();
            var viewModel = new UserListViewModel()
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title,
                UsersRows = new UsersRowsViewModel()
                {
                    SelectedColumns = JsonConvert.DeserializeObject<IEnumerable<ProfileColumnModel>>(json),
                    Users = GetActiveUsers(0, 0, model.DisplayedAmount)
                },
                UsersRowsViewPath = UsersRowsViewPath
            };
            return View(UserListViewPath, viewModel);
        }

        public virtual ActionResult GetUsers(int skip, int index, int count, string selectedColumns)
        {
            var model = new UsersRowsViewModel()
            {
                SelectedColumns = JsonConvert.DeserializeObject<IEnumerable<ProfileColumnModel>>(selectedColumns),
                Users = GetActiveUsers(skip, index, count)
            };
            return PartialView(UsersRowsViewPath, model);
        }

        private IEnumerable<UserModel> GetActiveUsers(int skip, int index, int count)
        {
            return _intranetUserService.GetAll().Where(i => !i.Inactive)
                .Select(MapToViewModel).OrderBy(i => i.DisplayedName)
                .Skip(skip + index * count).Take(count);
        }

        protected virtual UserModel MapToViewModel(IIntranetUser user)
        {
            var result = user.Map<UserModel>();
            return result;
        }
    }
}
