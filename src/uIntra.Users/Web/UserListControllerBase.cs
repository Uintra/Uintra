using LanguageExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Users.Helpers;
using Uintra.Users.UserList;
using Umbraco.Web.Mvc;
using static LanguageExt.Prelude;

namespace Uintra.Users.Web
{
	public abstract class UserListControllerBase : SurfaceController
	{
		protected virtual string UserListViewPath => @"~/App_Plugins/Users/UserList/UserListView.cshtml";
		protected virtual string UsersRowsViewPath => @"~/App_Plugins/Users/UserList/UsersRowsView.cshtml";
		protected virtual string UsersDetailsViewPath => @"~/App_Plugins/Users/UserList/UserDetailsPopup.cshtml";
        protected virtual string InviteUserRowViewPath => @"~/App_Plugins/Users/UserList/InviteUserRowView.cshtml";

		private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

		protected UserListControllerBase(IIntranetMemberService<IIntranetMember> intranetMemberService)
		{
			_intranetMemberService = intranetMemberService;
		}

		public virtual ActionResult Render(UserListModel model)
		{
			var selectedColumns = UsersPresentationHelper.GetProfileColumns().ToArray();

			var orderByColumn = selectedColumns.FirstOrDefault(i => i.SupportSorting);

			var groupId = Request.QueryString["groupId"].Apply(parseGuid).ToNullable();

			var viewModel = new UserListViewModel
			{
				AmountPerRequest = model.AmountPerRequest,
				DisplayedAmount = model.DisplayedAmount,
				Title = model.Title,
				MembersRows = GetUsersRowsViewModel(),
				OrderByColumn = orderByColumn
			};

			var activeUserSearchRequest = new ActiveMemberSearchQuery
			{
				Text = string.Empty,
				Skip = 0,
				Take = model.DisplayedAmount,
				OrderingString = orderByColumn?.PropertyName,
				GroupId = groupId,
				MembersOfGroup = groupId.HasValue

			};

			var (activeUsers, isLastRequest) = GetActiveUsers(activeUserSearchRequest);
			viewModel.MembersRows.SelectedColumns = UsersPresentationHelper.ExtendIfGroupMembersPage(groupId, selectedColumns);
			viewModel.MembersRows.Members = activeUsers;
			viewModel.IsLastRequest = isLastRequest;

			return View(UserListViewPath, viewModel);
		}

		public virtual ActionResult GetUsers(MembersListSearchModel listSearch)
		{
			var (activeUsers, isLastRequest) = GetActiveUsers(listSearch.Map<ActiveMemberSearchQuery>());

			var model = GetUsersRowsViewModel();

			model.SelectedColumns = UsersPresentationHelper.ExtendIfGroupMembersPage(listSearch.GroupId, UsersPresentationHelper.GetProfileColumns());
			model.Members = activeUsers;
			model.IsLastRequest = isLastRequest;

			return PartialView(UsersRowsViewPath, model);
		}

		//TODO Configure elastic for search among not invited users.
		public virtual ActionResult ForInvitation(MembersListSearchModel listSearch)
		{
			var (activeUsers, isLastRequest) = GetActiveUsers(listSearch.Map<ActiveMemberSearchQuery>());

			var model = GetUsersRowsViewModel();

			model.SelectedColumns = UsersPresentationHelper.AddManagementColumn(UsersPresentationHelper.GetProfileColumns());
			model.Members = activeUsers;
			model.IsLastRequest = isLastRequest;
			model.IsInvite = listSearch.IsInvite;

			return PartialView(InviteUserRowViewPath, model);
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
				var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

				viewResult.View.Render(viewContext, sw);

				viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

				return sw.GetStringBuilder().ToString();
			}
		}

		private (IEnumerable<MemberModel> result, bool isLastRequest) GetActiveUsers(ActiveMemberSearchQuery query)
		{
			var (searchResult, totalHits) = GetActiveUserIds(query);

			var result = searchResult
				.Apply(_intranetMemberService.GetMany)
				.Select(MapToViewModel);

			var isLastRequest = query.Skip + query.Take >= totalHits;

			return (result, isLastRequest);
		}

		protected abstract (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(ActiveMemberSearchQuery query);

		protected virtual MemberModel MapToViewModel(IIntranetMember user) =>
			user.Map<MemberModel>();

		protected virtual MembersRowsViewModel GetUsersRowsViewModel() =>
			new MembersRowsViewModel
			{
				SelectedColumns = UsersPresentationHelper.GetProfileColumns(),
			};

		public abstract bool ExcludeUserFromGroup(Guid groupId, Guid userId);

		[ChildActionOnly]
		public ActionResult RenderRows(MembersRowsViewModel model) =>
			View(UsersRowsViewPath, model);
	}
}
