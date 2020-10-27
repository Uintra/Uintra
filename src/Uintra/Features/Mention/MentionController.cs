using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Indexes;
using Uintra.Features.Links;
using Uintra.Features.Mention.Models;
using Uintra.Features.Search.Queries;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Mention
{
    public class MentionController : UBaselineApiController
    {
        private readonly IElasticIndex _elasticIndex;
        private readonly IProfileLinkProvider _profileLinkProvider;

        public MentionController(IElasticIndex elasticIndex, IProfileLinkProvider profileLinkProvider)
        {
            _elasticIndex = elasticIndex;
            _profileLinkProvider = profileLinkProvider;
        }

        [HttpGet]
        public IEnumerable<MentionUserModel> SearchMention(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = 5,
                SearchableTypeIds = UintraSearchableTypeEnum.Member.ToInt().ToListOfOne(),
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