using System;
using System.Linq;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Tagging.UserTags;
using Uintra.Tagging.UserTags.Models;

namespace Compent.Uintra.Core.UserTags
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
            searchableTag.Url = _searchUmbracoHelper.GetSearchPage().Url.AddQueryParameter(tag.Text);
            return searchableTag;
        }

        public void FillIndex(UserTag userTag)
        {
            var searchableTag = userTag.Map<SearchableTag>();
            _elasticTagIndex.Index(searchableTag);
        }

        public void Delete(Guid id)
        {
            _elasticTagIndex.Delete(id);
        }
    }
}