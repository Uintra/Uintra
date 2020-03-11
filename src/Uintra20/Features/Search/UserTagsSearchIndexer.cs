using System;
using System.Linq;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Search
{
    public class UserTagsSearchIndexer : IUserTagsSearchIndexer, IIndexer
    {
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly UserTagProvider _userTagProvider;
        private readonly IElasticTagIndex _elasticTagIndex;

        public UserTagsSearchIndexer(ISearchUmbracoHelper searchUmbracoHelper, UserTagProvider userTagProvider, IElasticTagIndex elasticTagIndex)
        {
            _searchUmbracoHelper = searchUmbracoHelper;
            _userTagProvider = userTagProvider;
            _elasticTagIndex = elasticTagIndex;
        }

        public void FillIndex()
        {
            var tags = _userTagProvider.GetAll();
            var searchableTags = tags.Select(Map);
            _elasticTagIndex.Index(searchableTags);
        }

        private SearchableTag Map(UserTag tag)
        {
            var searchableTag = tag.Map<SearchableTag>();
            searchableTag.Url = _searchUmbracoHelper.GetSearchPage().Url.AddQueryParameter(tag.Text).ToLinkModel();
            return searchableTag;
        }

        public void FillIndex(UserTag userTag)
        {
            var searchableTag = Map(userTag);
            _elasticTagIndex.Index(searchableTag);
        }

        public void Delete(Guid id)
        {
            _elasticTagIndex.Delete(id);
        }
    }
}