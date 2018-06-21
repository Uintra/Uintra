using System.Collections.Generic;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MentionApiController:MentionApiControllerBase
    {
        private readonly IElasticIndex _elasticIndex;

        public MentionApiController(IElasticIndex elasticIndex)
        {
            _elasticIndex = elasticIndex;
        }

        public override IEnumerable<MentionUserModel> SearchMention(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = 5,
                SearchableTypeIds = UintraSearchableTypeEnum.User.ToInt().ToListOfOne(),
                OnlyPinned = false,
                ApplyHighlights = false
            });
            
        }
    }
}