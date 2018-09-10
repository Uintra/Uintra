using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using Localization.Core;
using Uintra.Core.User;
using Uintra.Search;
using Uintra.Users.UserList;
using Uintra.Users.Web;


namespace Compent.Uintra.Controllers
{
    public class UserListController : UserListControllerBase
    {
        private readonly IElasticIndex _elasticIndex;
        private readonly ILocalizationCoreService _localizationCoreService;

        public UserListController(IIntranetUserService<IIntranetUser> intranetUserService,
            IElasticIndex elasticIndex,
            ILocalizationCoreService localizationCoreService
            )
            : base(intranetUserService)
        {
            _elasticIndex = elasticIndex;
            _localizationCoreService = localizationCoreService;
        }

        protected override IEnumerable<Guid> GetActiveUserIds(int skip, int take, string query, out long totalHits, string orderBy, int direction)
        {
            //TODO orderBy, direction (0-Asc/1-Desc)
            var searchQuery = new SearchTextQuery
            {
                Text = query,
                Skip = skip,
                Take = take,
                OrderingString = orderBy,
                OrderingDirection = direction,
                SearchableTypeIds = ((int) UintraSearchableTypeEnum.User).ToEnumerable()
            };
            var searchResult = _elasticIndex.Search(searchQuery);
            totalHits = searchResult.TotalHits;

            return searchResult.Documents.Select(r => r.Id.ToString().Pipe(Guid.Parse));
        }

        protected override string GetDetailsPopupTitle(UserModel user)
        {
            return $"{user.DisplayedName} {_localizationCoreService.Get("UserList.DetailsPopup.Title")}";
        }
    }
}