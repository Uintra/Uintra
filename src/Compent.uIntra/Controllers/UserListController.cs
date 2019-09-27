using Compent.Extensions;
using Compent.LinkPreview.HttpClient.Extensions;
using Compent.Uintra.Core.Search.Entities;
using EmailWorker.Data.Extensions;
using LanguageExt;
using Localization.Core;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Attributes;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Search;
using Uintra.Users.UserList;
using Uintra.Users.Web;
using static LanguageExt.Prelude;
using static System.Net.HttpStatusCode;
using static Uintra.Groups.GroupModelGetters;

namespace Compent.Uintra.Controllers
{
	[ThreadCulture]
	public class UserListController : UserListControllerBase
	{
		private readonly IElasticMemberIndex _elasticIndex;
		private readonly ILocalizationCoreService _localizationCoreService;
		private readonly IProfileLinkProvider _profileLinkProvider;
		private readonly IGroupService _groupService;
		private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
		private readonly IGroupMemberService _groupMemberService;
		private readonly INotificationsService _notificationsService;

		public UserListController(
			IIntranetMemberService<IIntranetMember> intranetMemberService,
			IElasticMemberIndex elasticIndex,
			ILocalizationCoreService localizationCoreService,
			IProfileLinkProvider profileLinkProvider,
			IGroupService groupService,
			IGroupMemberService groupMemberService,
			INotificationsService notificationsService)
			: base(intranetMemberService)
		{
			_elasticIndex = elasticIndex;
			_localizationCoreService = localizationCoreService;
			_profileLinkProvider = profileLinkProvider;
			_groupService = groupService;
			_intranetMemberService = intranetMemberService;
			_groupMemberService = groupMemberService;
			_notificationsService = notificationsService;
		}

		[NotFoundGroup]
		public override ActionResult Render(UserListModel model)
		{
			return base.Render(model);
		}

		protected override (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(
			ActiveMemberSearchQuery query)
		{
			var searchQuery = new MemberSearchQuery
			{
				Text = query.Text,
				Skip = query.Skip,
				Take = query.Take,
				OrderingString = query.OrderingString,
				SearchableTypeIds = ((int)UintraSearchableTypeEnum.User).ToEnumerable(),
				GroupId = query.GroupId,
				MembersOfGroup = query.MembersOfGroup
			};

			var searchResult = _elasticIndex.Search(searchQuery);
			var result = searchResult.Documents.Select(r => Guid.Parse(r.Id.ToString()));

			return (result, searchResult.TotalHits);
		}

		protected override string GetDetailsPopupTitle(MemberModel user) =>
			$"{user.DisplayedName} {_localizationCoreService.Get("UserList.DetailsPopup.Title")}";

		protected override MemberModel MapToViewModel(IIntranetMember user)
		{
			var model = base.MapToViewModel(user);
			model.ProfileUrl = _profileLinkProvider.GetProfileLink(user.Id);

			var isAdmin = _groupMemberService
				.IsMemberAdminOfGroup(user.Id, CurrentGroup()
					.Match(Some: GroupId, None: () => Guid.Empty));

			model.IsGroupAdmin = isAdmin;

			return model;
		}

		protected override MembersRowsViewModel GetUsersRowsViewModel()
		{
			var model = base.GetUsersRowsViewModel();
			model.CurrentMember = _intranetMemberService.GetCurrentMember().Map<MemberViewModel>();

			model.IsCurrentMemberGroupAdmin = _groupMemberService
				.IsMemberAdminOfGroup(model.CurrentMember.Id, CurrentGroup()
					.Match(Some: GroupId, None: () => Guid.Empty));

			model.GroupId = CurrentGroup().Match(Some: GroupId, None: () => Guid.Empty);

			return model;
		}

		public override bool ExcludeUserFromGroup(Guid groupId, Guid userId)
		{
			var currentUserId = _intranetMemberService.GetCurrentMember().Id;
			var group = _groupService.Get(groupId);

			if (currentUserId == group.CreatorId || userId == currentUserId)
			{
				_groupMemberService.Remove(groupId, userId);
				return true;
			}

			return false;
		}


		[HttpPut]
		public ActionResult Assign(GroupToggleAdminRightsModel rights)
		{
			_groupMemberService.ToggleAdminRights(rights.MemberId, rights.GroupId);

			return new HttpStatusCodeResult(OK);
		}

		[HttpPost]
        public ActionResult InviteMember(MemberGroupInviteModel invite)
        {
            InviteUser(invite);
            SendInvitationToUser(invite);
            UpdateCache(invite);

            return new HttpStatusCodeResult(OK);
        }

        private void UpdateCache(MemberGroupInviteModel invite)
        {
            var casted = _intranetMemberService as IntranetMemberService<IntranetMember>;

            casted.UpdateMemberCache(invite.MemberId);
        }

		private void InviteUser(MemberGroupInviteModel invite) =>
			_groupMemberService.Add(invite.GroupId, new GroupMemberSubscriptionModel
			{
				MemberId = invite.MemberId
			});

		private void SendInvitationToUser(MemberGroupInviteModel invite) =>
			_notificationsService.ProcessNotification(new NotifierData
			{
				NotificationType = NotificationTypeEnum.GroupInvitation,
				ReceiverIds = List(invite.MemberId),
				ActivityType = CommunicationTypeEnum.Group
			});

		private static Option<Guid> CurrentGroupId()
		{
			var result = System.Web.HttpContext.Current.Request.Params["groupId"].Apply(parseGuid);

			return result.IsNone
				? GetFromBody(System.Web.HttpContext.Current.Request, result)
				: result;
		}

		private static Option<Guid> GetFromBody(HttpRequest request, Option<Guid> noneResult)
		{
			var bodyStream = new StreamReader(request.InputStream);
			bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
			var bodyText = bodyStream.ReadToEnd();
			var queryModel = bodyText.Deserialize<MembersListSearchModel>();
			return queryModel?.GroupId ?? noneResult;
		}

		private Option<GroupModel> CurrentGroup() =>
			CurrentGroupId().Map(_groupService.Get);
	}
}