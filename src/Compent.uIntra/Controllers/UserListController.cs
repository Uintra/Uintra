using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
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
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IGroupMemberService _groupMemberService;

        public UserListController(IIntranetUserService<IIntranetUser> intranetUserService,
            IElasticIndex elasticIndex,
            ILocalizationCoreService localizationCoreService,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService,
            IGroupMemberService groupMemberService
            )
            : base(intranetUserService)
        {
            _elasticIndex = elasticIndex;
            _localizationCoreService = localizationCoreService;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            _intranetUserService = intranetUserService;
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

        protected override string GetDetailsPopupTitle(UserModel user) =>
            $"{user.DisplayedName} {_localizationCoreService.Get("UserList.DetailsPopup.Title")}";

        protected override UserModel MapToViewModel(IIntranetUser user)
        {
            var model = base.MapToViewModel(user);
            model.ProfileUrl = _profileLinkProvider.GetProfileLink(user.Id);
            model.IsGroupAdmin = CurrentGroup().Map(CreatorId) == user.Id;
            return model;
        }

        protected override UsersRowsViewModel GetUsersRowsViewModel()
        {
            var model = base.GetUsersRowsViewModel();
            model.CurrentUser = _intranetUserService.GetCurrentUser().Map<UserViewModel>();
            model.IsCurrentUserAdmin = CurrentGroup().Map(CreatorId) == model.CurrentUser.Id;
            return model;
        }

        public override bool ExcludeUserFromGroup(Guid userId)
        {
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var currentGroupCreatorId = CurrentGroup().Map(CreatorId);

            return currentGroupCreatorId
                .Filter(creatorId => currentUserId.In(userId, creatorId) && currentUserId != creatorId)
                .Match(
                    Some: groupId =>
                    {
                        _groupMemberService.Remove(groupId, userId);
                        return true;
                    },
                    None: () => false);
        }

        private Option<Guid> CurrentGroupId()
        {
            var result =
             System.Web.HttpContext.Current.Request
                .Params["groupId"]
                .Apply(parseGuid);
            if (result.IsNone)
                result = GroupId.HasValue ? Some(GroupId.Value) : None;
            return result;
        }

        private Guid CreatorId(GroupModel group) => group.CreatorId;

        private Option<GroupModel> CurrentGroup() =>
            CurrentGroupId().Map(_groupService.Get);
    }
}