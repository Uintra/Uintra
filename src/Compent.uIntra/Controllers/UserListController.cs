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
        private GroupModel _currentGroup;

        public UserListController(IIntranetUserService<IIntranetUser> intranetUserService,
            IElasticIndex elasticIndex,
            ILocalizationCoreService localizationCoreService,
            IProfileLinkProvider profileLinkProvider,
            IGroupService groupService
            )
            : base(intranetUserService)
        {
            _elasticIndex = elasticIndex;
            _localizationCoreService = localizationCoreService;
            _profileLinkProvider = profileLinkProvider;
            _groupService = groupService;
            var groupId = global::System.Web.HttpContext.Current.Request.QueryString["groupId"];
            _currentGroup = Guid.TryParse(groupId, out Guid result) ?
                groupService.Get(result) : null;
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
    }
}