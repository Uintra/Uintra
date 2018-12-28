using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using LanguageExt;
using Localization.Core;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Search;
using Uintra.Users.UserList;
using Uintra.Users.Web;


namespace Compent.Uintra.Controllers
{
    public class UserListController : UserListControllerBase
    {
        private readonly IElasticIndex _elasticIndex;
        private readonly ILocalizationCoreService _localizationCoreService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupService _groupService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IGroupMemberService _groupMemberService;
        private GroupModel _currentGroup;

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
            _currentGroup = GetCurrentGroup();
        }

        protected override IEnumerable<Guid> GetActiveUserIds(int skip, int take, string query, string groupId, out long totalHits, string orderBy, int direction)
        {
            var searchQuery = new SearchTextQuery
            {
                Text = query,
                Skip = skip,
                Take = take,
                OrderingString = orderBy,
                OrderingDirection = direction,
                SearchableTypeIds = ((int)UintraSearchableTypeEnum.User).ToEnumerable(),
                GroupId = groupId
            };
            var searchResult = _elasticIndex.Search(searchQuery);
            totalHits = searchResult.TotalHits;

            return searchResult.Documents.Select(r => r.Id.ToString().Apply(Guid.Parse));
        }

        protected override string GetDetailsPopupTitle(UserModel user)
        {
            return $"{user.DisplayedName} {_localizationCoreService.Get("UserList.DetailsPopup.Title")}";
        }

        protected override UserModel MapToViewModel(IIntranetUser user)
        {
            var model = base.MapToViewModel(user);
            model.ProfileUrl = _profileLinkProvider.GetProfileLink(model.Id);
            model.IsGroupAdmin = _currentGroup?.CreatorId == user.Id;
            return model;
        }

        protected override UsersRowsViewModel GetUsersRowsViewModel()
        {
            var model = base.GetUsersRowsViewModel();
            model.CurrentUser = _intranetUserService.GetCurrentUser();
            model.IsCurrentUserAdmin = _currentGroup?.CreatorId == model.CurrentUser.Id;
            return model;
        }

        public override bool ExcludeUserFromGroup(Guid userId)
        {
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var currentGroupCreatorId = _currentGroup?.CreatorId;
            //Model.CurrentUser.Id == user.Id || Model.IsCurrentUserAdmin) && !user.IsGroupAdmin
            if ((currentUserId == userId || currentUserId == currentGroupCreatorId) && currentUserId != currentGroupCreatorId)
            {
                _groupMemberService.Remove(_currentGroup.Id, userId);
                return true;
            }
            return false;
        }

        private GroupModel GetCurrentGroup()
        {
            var groupId = System.Web.HttpContext.Current.Request.QueryString["groupId"] ??
                System.Web.HttpContext.Current.Request.UrlReferrer.Query.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            return Guid.TryParse(groupId, out Guid result) ? _groupService.Get(result) : null;
        }
    }
}