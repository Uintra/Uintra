using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Search;
using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MentionController : MentionControllerBase
    {        
        private readonly IElasticIndex _elasticIndex;
        private readonly IProfileLinkProvider _profileLinkProvider;

        public MentionController(IElasticIndex elasticIndex, IProfileLinkProvider profileLinkProvider)
        {
            _elasticIndex = elasticIndex;
            _profileLinkProvider = profileLinkProvider;
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
                var searchableUser = (SearchableMember)d;
                var user = new MentionUserModel()
                {

                    Id = Guid.Parse(searchableUser.Id.ToString()),
                    Value = searchableUser.FullName
                };
                user.Url = _profileLinkProvider.GetProfileLink(user.Id);
                return user;
            });

            return results;

        }
    }
}