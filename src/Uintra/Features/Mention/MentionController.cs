using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Queries.SearchByText;
using Uintra.Core.Search.Repository;
using Uintra.Features.Links;
using Uintra.Features.Mention.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Mention
{
    public class MentionController : UBaselineApiController
    {
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IUintraSearchRepository _searchRepository;

        public MentionController(
            IProfileLinkProvider profileLinkProvider,
            IUintraSearchRepository searchRepository)
        {
            _profileLinkProvider = profileLinkProvider;
            _searchRepository = searchRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<MentionUserModel>> SearchMention(string query)
        {
            var searchByTextQuery = new SearchByTextQuery
            {
                Text = query,
                Take = 5,
                SearchableTypeIds = UintraSearchableTypeEnum.Member.ToInt().ToListOfOne(),
                OnlyPinned = false,
                ApplyHighlights = false
            };
            var searchResult = await _searchRepository.SearchAsyncTyped(searchByTextQuery);
            
            var results = searchResult.Documents.Select(d =>
            {
                var searchableUser = (SearchableMember)d;
                var user = new MentionUserModel()
                {
                    Id = Guid.Parse(searchableUser.Id),
                    Value = searchableUser.FullName
                };
                user.Url = _profileLinkProvider.GetProfileLink(user.Id);
                return user;
            });

            return results;
        }
    }
}