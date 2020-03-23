using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Links;
using Uintra20.Features.Mention.Models;
using Uintra20.Features.Search.Queries;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Mention
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