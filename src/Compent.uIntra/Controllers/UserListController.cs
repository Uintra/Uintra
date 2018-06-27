using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.User;
using Uintra.Search;
using Uintra.Users.Web;


namespace Compent.Uintra.Controllers
{
    public class UserListController : UserListControllerBase
    {
        private readonly IElasticIndex _elasticIndex;

        public UserListController(IIntranetUserService<IIntranetUser> intranetUserService, IElasticIndex elasticIndex)
            : base(intranetUserService)
        {
            _elasticIndex = elasticIndex;
        }

        protected override IEnumerable<Guid> GetActiveUserIds(int skip, int take, string query, string orderBy, int direction)
        {
            //TODO orderBy, direction (0-Asc/1-Desc)
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Skip = skip,
                Take = take,
                //OrderingString = orderBy,
                OrderingDirection = direction,
                SearchableTypeIds = ((int) UintraSearchableTypeEnum.User).ToEnumerable()
            });

            return searchResult.Documents.Select(r => r.Id.ToString().Pipe(Guid.Parse));
        }
    }
}