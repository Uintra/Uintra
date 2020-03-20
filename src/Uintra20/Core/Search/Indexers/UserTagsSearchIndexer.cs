using System;
using System.Linq;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Helpers;
using Uintra20.Core.Search.Indexers.Diagnostics;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Search.Web;
using Uintra20.Features.Social;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Search.Indexers
{
    public class UserTagsSearchIndexer : IUserTagsSearchIndexer, IIndexer
    {
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly UserTagProvider _userTagProvider;
        private readonly IElasticTagIndex _elasticTagIndex;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService; 

        public UserTagsSearchIndexer(
            ISearchUmbracoHelper searchUmbracoHelper, 
            UserTagProvider userTagProvider, 
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