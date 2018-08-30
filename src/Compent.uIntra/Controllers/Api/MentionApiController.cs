using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Extensions;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MentionApiController : MentionApiControllerBase
    {
        private readonly IElasticIndex _elasticIndex;

        public MentionApiController(IElasticIndex elasticIndex)
        {
            _elasticIndex = elasticIndex;
        }

        [HttpGet]
        public override IEnumerable<MentionUserModel> SearchMention(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = 5, //TODO move to appSettings key
                SearchableTypeIds = UintraSearchableTypeEnum.User.ToInt().ToEnumerable()
            });

            var searchableUsers = searchResult.Documents
                .Cast<SearchableUser>()
                .Map<IEnumerable<MentionUserModel>>();

            return searchableUsers;
        }
    }
}