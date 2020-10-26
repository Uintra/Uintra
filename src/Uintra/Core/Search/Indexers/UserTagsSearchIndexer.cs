using System;
using System.Linq;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Core.Search.Indexers.Diagnostics.Models;
using Uintra.Core.Search.Indexes;
using Uintra.Features.Search.Web;
using Uintra.Features.Social;
using Uintra.Features.Tagging.UserTags.Models;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Indexers
{
    public class UserTagsSearchIndexer : IUserTagsSearchIndexer, IIndexer
    {
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IUserTagProvider _userTagProvider;
        private readonly IElasticTagIndex _elasticTagIndex;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService; 

        public UserTagsSearchIndexer(
            ISearchUmbracoHelper searchUmbracoHelper, 
            IUserTagProvider userTagProvider, 
            IElasticTagIndex elasticTagIndex, 
            IIndexerDiagnosticService indexerDiagnosticService)
        {
            _searchUmbracoHelper = searchUmbracoHelper;
            _userTagProvider = userTagProvider;
            _elasticTagIndex = elasticTagIndex;
            _indexerDiagnosticService = indexerDiagnosticService;
        }

        public IndexedModelResult FillIndex()
        {
            try
            {
                var tags = _userTagProvider.GetAll();
                var searchableTags = tags.Select(Map).ToList();
                _elasticTagIndex.Index(searchableTags);

                return _indexerDiagnosticService.GetSuccessResult(typeof(UserTagsSearchIndexer).Name, searchableTags);
            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(UserTagsSearchIndexer).Name);
            }
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