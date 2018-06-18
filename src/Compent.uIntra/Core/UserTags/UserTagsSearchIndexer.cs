using System.Linq;
using Compent.uIntra.Core.Search.Entities;
using uIntra.Core.Extensions;
using uIntra.Search;
using uIntra.Tagging.UserTags;
using uIntra.Tagging.UserTags.Models;

namespace Compent.uIntra.Core.UserTags
{
    public class UserTagsSearchIndexer : IIndexer
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
    }
}