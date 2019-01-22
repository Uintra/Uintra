using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.Users.UserList;
using EmailWorker.Data.Extensions;
using LanguageExt;
using Localization.Core;
using Localization.Umbraco.Attributes;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Attributes;
using Uintra.Search;
using Uintra.Users.UserList;
using Uintra.Users.Web;
using static LanguageExt.Prelude;

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

        protected override (IEnumerable<Guid> searchResult, long totalHits) GetActiveUserIds(ActiveUserSearchQuery query)
        {
            var searchQuery = new SearchTextQuery
            {
                Text = query.Text,
                Skip = query.Skip,
                Take = query.Take,
                OrderingString = query.OrderingString,
                OrderingDirection = query.OrderingDirection,
                SearchableTypeIds = ((int)UintraSearchableTypeEnum.User).ToEnumerable(),
                GroupId = query.GroupId
            };

            var searchResult = _elasticIndex.Search(searchQuery);
            var result = searchResult.Documents.Select(r => Guid.Parse(r.Id.ToString()));

            return (result, searchResult.TotalHits);
        }

        protected override string GetDetailsPopupTitle(MemberModel member) => 
            $"{member.DisplayedName} {_localizationCoreService.Get("UserList.DetailsPopup.Title")}";

        protected override MemberModel MapToViewModel(IIntranetMember member)
        {
            var model = base.MapToViewModel(member);
            model.ProfileUrl = _profileLinkProvider.GetProfileLink(member.Id);
            model.IsGroupAdmin = CurrentGroup().Map(CreatorId) == member.Id;
            return model;
        }

        protected override MembersRowsViewModel GetUsersRowsViewModel()
        {
            var model = base.GetUsersRowsViewModel();
            model.CurrentMember = _intranetMemberService.GetCurrentMember().Map<MemberViewModel>();
            model.IsCurrentMemberAdmin = CurrentGroup().Map(CreatorId) == model.CurrentMember.Id;
            return model;
        }

        public override bool ExcludeUserFromGroup(Guid userId)
        {
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            var currentGroupCreatorId = CurrentGroup().Map(CreatorId);

            return currentGroupCreatorId
                .Filter(creatorId => currentMemberId.In(userId, creatorId) && currentMemberId != creatorId)
                .Match(
                    Some: groupId =>
                    {
                        _groupMemberService.Remove(groupId, userId);
                        return true;
                    },
                    None: () => false);
        }

        private static Option<Guid> CurrentGroupId() =>
            System.Web.HttpContext.Current.Request
                .Params["groupId"]
                .Apply(parseGuid);

        private static Guid CreatorId(GroupModel group) => group.CreatorId;

        private Option<GroupModel> CurrentGroup() =>
            CurrentGroupId().Map(_groupService.Get);
    }
}