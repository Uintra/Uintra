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
            string json = (string)model.SelectedProperties.ToString();
            var viewModel = new UserListViewModel()
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title,
                SelectedColumns = JArray.Parse(json).Children()
                    .Select(i => new ProfileColumnModel()
                    {
                        Id = i.Value<int>("id"),
                        Name = i.Value<string>("name"),
                        Type = (ColumnType)Enum.Parse(typeof(ColumnType), i.Value<string>("type")),
                        Alias = i.Value<string>("alias")
                    }),
                Users = GetActiveUsers(0, model.DisplayedAmount)
            };
            return View(ViewPath, viewModel);
        }

        private IEnumerable<UserModel> GetActiveUsers(int index, int count)
        {
            return _intranetUserService.GetAll().Where(i => !i.Inactive)
                .Select(MapToViewModel).OrderBy(i => i.DisplayedName)
                .Skip(index*count).Take(count);
        }

        protected virtual UserModel MapToViewModel(IIntranetUser user)
        {
            var result = user.Map<UserModel>();
            return result;
        }
    }
}
