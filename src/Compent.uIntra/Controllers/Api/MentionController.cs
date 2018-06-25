using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MentionController : MentionControllerBase
    {
        //regex for getting  mentioned user id
        //(?<=\bdata-id=")[^"]*

        private readonly IElasticIndex _elasticIndex;

        public MentionController(IElasticIndex elasticIndex)
        {
            _elasticIndex = elasticIndex;
        }

        [HttpGet]
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

            var results = searchResult.Documents.Select(d =>
            {
                var searchableUser = (SearchableUser)d;
                var user = new MentionUserModel()
                {

                    Id = Guid.Parse(searchableUser.Id.ToString()),
                    Value = searchableUser.FullName
                };
                return user;
            });

            return results;

        }
    }
}