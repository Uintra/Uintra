using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Web.Mvc;
using Newtonsoft.Json;
using Uintra.Users.UserList;
using System.IO;

namespace Uintra.Users.Web
{
    public abstract class UserListControllerBase : SurfaceController
    {
        protected virtual string UserListViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";
        protected virtual string UsersRowsViewPath => @"~/App_Plugins/Users/UserList/UsersRowsView.cshtml";
        protected virtual string UsersDetailsViewPath => @"~/App_Plugins/Users/UserList/UserDetailsPopup.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected UserListControllerBase(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public virtual ActionResult Render(UserListModel model)
        {
            string selectedPropertiesJson = (string)model.SelectedProperties.ToString();
            string orderBycolumnJson = model.OrderBy == null ? null : (string)model.OrderBy.ToString();
            var selecetedColumns = JsonConvert.DeserializeObject<IEnumerable<ProfileColumnModel>>(selectedPropertiesJson);
            var orderByColumn = orderBycolumnJson == null ?
                selecetedColumns.OrderBy(i => i.Id).FirstOrDefault(i => i.SupportSorting) :
                JsonConvert.DeserializeObject<ProfileColumnModel>(orderBycolumnJson);
            var groupId = Request.QueryString["groupId"];
            selecetedColumns = ExtendIfGroup(groupId, selecetedColumns);
            var viewModel = new UserListViewModel()
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title,
                UsersRows = new UsersRowsViewModel()
                {
                    SelectedColumns = selecetedColumns,
                    Users = GetActiveUsers(0, model.DisplayedAmount, orderByColumn?.PropertyName, 0, groupId, out var isLastRequest)
                },
                UsersRowsViewPath = UsersRowsViewPath,
                OrderByColumn = orderByColumn,
                IsLastRequest = isLastRequest
            };
            return View(UserListViewPath, viewModel);
        }

        public virtual ActionResult GetUsers(int skip, int take, string query, string selectedColumns, string orderBy, int direction, string groupId)
        {
            var columns = JsonConvert.DeserializeObject<IEnumerable<ProfileColumnModel>>(selectedColumns);
            var model = new UsersRowsViewModel
            {
                SelectedColumns = columns,
                Users = GetActiveUsers(skip, take, orderBy, direction, groupId, out var isLastRequest, query),
                IsLastRequest = isLastRequest
            };
            return PartialView(UsersRowsViewPath, model);
        }

        [HttpPost]
        public virtual JsonNetResult Details(Guid id)
        {
            var user = _intranetUserService.Get(id);
            var viewModel = MapToViewModel(user);
            var text = RenderPartialViewToString(UsersDetailsViewPath, viewModel);
            var title = GetDetailsPopupTitle(viewModel);
            return new JsonNetResult()
            {
                Data = new DetailsPopupModel
                {
                    Title = title,
                    Text = text
                }
            };
        }

        protected virtual string GetDetailsPopupTitle(UserModel user)
        {
            return user.DisplayedName;
        }

        private string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(
                    ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                    ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private IEnumerable<UserModel> GetActiveUsers(int skip, int take, string orderBy, int direction, string groupId, out bool isLastRequest,  string query = "" )
        {
            var result = GetActiveUserIds(skip, take, query, groupId, out var totalHits, orderBy, direction)
                .Pipe(_intranetUserService.GetMany)
                .Select(MapToViewModel);
            isLastRequest = skip + take >= totalHits;
            return result;
        }

        protected abstract IEnumerable<Guid> GetActiveUserIds(int skip, int take, string query, string groupId, out long totalHits, string orderBy = null, int direction = 0);

        protected virtual UserModel MapToViewModel(IIntranetUser user)
        {
            var result = user.Map<UserModel>();
            return result;
        }

        private IEnumerable<ProfileColumnModel> ExtendIfGroup(string groupId, IEnumerable<ProfileColumnModel> columns)
        {
            if (string.IsNullOrWhiteSpace(groupId)) return columns;
            return columns.Append(new ProfileColumnModel()
            {
                Alias = "Role",
                Id = 99,
                Name = "Role",
                PropertyName = "role",
                SupportSorting = false,
                Type = ColumnType.GroupRole
            });
        }
    }
}
