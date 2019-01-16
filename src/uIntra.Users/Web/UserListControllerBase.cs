using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Web.Mvc;
using Newtonsoft.Json;
using Uintra.Users.UserList;
using System.IO;
using LanguageExt;
using static LanguageExt.Prelude;

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
            var selectedPropertiesJson = (string) model.SelectedProperties.ToString();
            var orderByColumnJson = model.OrderBy is null ? None : Some((string)model.OrderBy.ToString());
            var selectedColumns = selectedPropertiesJson.Deserialize<List<ProfileColumnModel>>();

            var orderByColumn = orderByColumnJson
                .Map(SerializationExtensions.Deserialize<ProfileColumnModel>)
                .IfNone(() => selectedColumns.OrderBy(i => i.Id).FirstOrDefault(i => i.SupportSorting));

            var groupId = Request.QueryString["groupId"].Apply(parseGuid);
   
            var viewModel = new UserListViewModel
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title,
                UsersRows = GetUsersRowsViewModel(),
                UsersRowsViewPath = UsersRowsViewPath,
                OrderByColumn = orderByColumn
            };

            var activeUserSearchRequest = new ActiveUserSearchQuery
            {
                Text = string.Empty,
                Skip = 0,
                Take = model.DisplayedAmount,
                OrderingString = orderByColumn?.PropertyName,
                OrderingDirection = 0,
                GroupId = groupId
            };
                

            var (activeUsers, isLastRequest) = GetActiveUsers(activeUserSearchRequest);
            viewModel.UsersRows.SelectedColumns = ExtendIfGroupMembersPage(groupId, selectedColumns);
            viewModel.UsersRows.Users = activeUsers;
            viewModel.IsLastRequest = isLastRequest;
            return View(UserListViewPath, viewModel);
        }

        public virtual ActionResult GetUsers(ActiveUserSearchQueryModel queryModel)
        {
            var columns = JsonConvert.DeserializeObject<IEnumerable<ProfileColumnModel>>(queryModel.SelectedColumns);
            var model = GetUsersRowsViewModel();
            model.SelectedColumns = columns;

            var query = queryModel.Map<ActiveUserSearchQuery>();
            var (activeUsers, isLastRequest) = GetActiveUsers(query);

            model.Users = activeUsers;
            model.IsLastRequest = isLastRequest;
            return PartialView(UsersRowsViewPath, model);
        }

        [HttpPost]
        public virtual JsonNetResult Details(Guid id)
        {
            var user = _intranetUserService.Get(id);
            var viewModel = MapToViewModel(user);
            var text = RenderPartialViewToString(UsersDetailsViewPath, viewModel);
            var title = GetDetailsPopupTitle(viewModel);
            return new JsonNetResult
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

        private (IEnumerable<UserModel> result, bool isLastRequest) GetActiveUsers(ActiveUserSearchQuery query)
        {
            var (searchResult, totalHits) = GetActiveUserIds(query);

            var result = searchResult
                .Apply(_intranetUserService.GetMany)
                .Select(MapToViewModel);

            var isLastRequest = query.Skip + query.Take >= totalHits;

            return (result, isLastRequest);
        }

        protected abstract (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(ActiveUserSearchQuery query);

        protected virtual UserModel MapToViewModel(IIntranetUser user)
        {
            var result = user.Map<UserModel>();
            return result;
        }

        private static IEnumerable<ProfileColumnModel> ExtendIfGroupMembersPage(Option<Guid> groupId, IEnumerable<ProfileColumnModel> columns)
        {
            if (groupId.IsNone) return columns;
            return columns.Append(new ProfileColumnModel
            {
                Alias = "Role",
                Id = 99,
                Name = "Role",
                PropertyName = "role",
                SupportSorting = false,
                Type = ColumnType.GroupRole
            });
        }

        protected virtual UsersRowsViewModel GetUsersRowsViewModel() => 
            new UsersRowsViewModel();

        public abstract bool ExcludeUserFromGroup(Guid userId);
    }
}
