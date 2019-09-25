using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using EmailWorker.Data.Extensions;
using LanguageExt;
using Localization.Core;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Attributes;
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
        private readonly IElasticIndex _elasticIndex;
        private readonly ILocalizationCoreService _localizationCoreService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IGroupMemberService _groupMemberService;

        public UserListController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IElasticIndex elasticIndex,
            ILocalizationCoreService localizationCoreService,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService
        )
            : base(intranetMemberService)
        {
            _elasticIndex = elasticIndex;
            _localizationCoreService = localizationCoreService;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetMemberService = intranetMemberService;
            _groupMemberService = groupMemberService;
        }

        [NotFoundGroup]
        public override ActionResult Render(UserListModel model)
        {
            return base.Render(model);
        }

        protected override (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(
            ActiveUserSearchQuery query)
        {
            var searchQuery = new SearchTextQuery
            {
                Text = query.Text,
                Skip = query.Skip,
                Take = query.Take,
                OrderingString = query.OrderingString,
                SearchableTypeIds = ((int) UintraSearchableTypeEnum.User).ToEnumerable(),
                GroupId = query.GroupId
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
            if (!ModelState.IsValid) return new HttpStatusCodeResult(BadRequest);

            _groupMemberService.ToggleAdminRights(rights.MemberId, rights.GroupId);

            return new HttpStatusCodeResult(OK);
        }

        private static Option<Guid> CurrentGroupId()
        {
            var result =
                System.Web.HttpContext.Current.Request
                    .Params["groupId"]
                    .Apply(parseGuid);
            return result;
        }

        private Option<GroupModel> CurrentGroup() =>
            CurrentGroupId().Map(_groupService.Get);
    }
}