using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Umbraco.Web.Mvc;
using Uintra.Users.UserList;
using System.IO;
using LanguageExt;
using static LanguageExt.Prelude;
using Uintra.Users.Helpers;

namespace Uintra.Users.Web
{
    public abstract class UserListControllerBase : SurfaceController
    {
        protected virtual string UserListViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";
        protected virtual string UsersRowsViewPath => @"~/App_Plugins/Users/UserList/UsersRowsView.cshtml";
        protected virtual string UsersDetailsViewPath => @"~/App_Plugins/Users/UserList/UserDetailsPopup.cshtml";
        
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected UserListControllerBase(IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }

        public virtual ActionResult Render(UserListModel model)
        {
            var selectedColumns = ReflectionHelper.GetProfileColumns().ToArray();
            var orderByColumn = selectedColumns.FirstOrDefault(i => i.SupportSorting);

            var groupId = Request.QueryString["groupId"].Apply(parseGuid);

            var viewModel = new UserListViewModel
            {
                AmountPerRequest = model.AmountPerRequest,
                DisplayedAmount = model.DisplayedAmount,
                Title = model.Title,
                MembersRows = GetUsersRowsViewModel(),
                OrderByColumn = orderByColumn
            };

            var activeUserSearchRequest = new ActiveUserSearchQuery
            {
                Text = string.Empty,
                Skip = 0,
                Take = model.DisplayedAmount,
                OrderingString = orderByColumn?.PropertyName,
                GroupId = groupId
            };

            var (activeUsers, isLastRequest) = GetActiveUsers(activeUserSearchRequest);
            viewModel.MembersRows.SelectedColumns = ExtendIfGroupMembersPage(groupId, selectedColumns);
            viewModel.MembersRows.Members = activeUsers;
            viewModel.IsLastRequest = isLastRequest;
            return View(UserListViewPath, viewModel);
        }

        public virtual ActionResult GetUsers(ActiveUserSearchQueryModel queryModel)
        {            
            var model = GetUsersRowsViewModel();

            var query = queryModel.Map<ActiveUserSearchQuery>();
            var (activeUsers, isLastRequest) = GetActiveUsers(query);

            model.SelectedColumns = ExtendIfGroupMembersPage(queryModel.GroupId.ToOption(), ReflectionHelper.GetProfileColumns());
            model.Members = activeUsers;
            model.IsLastRequest = isLastRequest;

            return PartialView(UsersRowsViewPath, model);
        }

        [HttpPost]
        public virtual JsonNetResult Details(Guid id)
        {
            var user = _intranetMemberService.Get(id);
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

        protected virtual string GetDetailsPopupTitle(MemberModel user)
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

        private (IEnumerable<MemberModel> result, bool isLastRequest) GetActiveUsers(ActiveUserSearchQuery query)
        {
            var (searchResult, totalHits) = GetActiveUserIds(query);

            var result = searchResult
                .Apply(_intranetMemberService.GetMany)
                .Select(MapToViewModel);

            var isLastRequest = query.Skip + query.Take >= totalHits;

            return (result, isLastRequest);
        }

        protected abstract (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(ActiveUserSearchQuery query);

        protected virtual MemberModel MapToViewModel(IIntranetMember user)
        {
            var result = user.Map<MemberModel>();
            return result;
        }

        private static IEnumerable<ProfileColumnModel> ExtendIfGroupMembersPage(Option<Guid> groupId, IEnumerable<ProfileColumnModel> columns)
        {
            if (groupId.IsNone) return columns;
            return columns.Append(new ProfileColumnModel
            {
                Alias = "Group",
                Id = 99,
                Name = "Group",
                PropertyName = "role",
                SupportSorting = false,
                Type = ColumnType.GroupRole
            }).Append(new ProfileColumnModel
            {
                Alias = "Management",
                Id = 100,
                Name = "Management",
                PropertyName = "management",
                SupportSorting = false,
                Type = ColumnType.GroupManagement
            });
        }

        protected virtual MembersRowsViewModel GetUsersRowsViewModel() =>
            new MembersRowsViewModel
            {
                SelectedColumns = ReflectionHelper.GetProfileColumns()          
            };

        public abstract bool ExcludeUserFromGroup(Guid groupId, Guid userId);

        [ChildActionOnly]
        public ActionResult RenderRows(MembersRowsViewModel model)
        {
            return View(UsersRowsViewPath, model);
        }
    }
}
