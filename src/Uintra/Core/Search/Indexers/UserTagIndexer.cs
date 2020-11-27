using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Features.Tagging.UserTags.Models;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Search.Indexers
{
    public class UserTagIndexer : IUserTagIndexer
    {
        public Type Type { get; } = typeof(SearchableTag);

        private readonly IUserTagProvider _userTagProvider;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IIndexContext<SearchableTag> _indexContext;
        private readonly ISearchRepository<SearchableTag> _searchRepository;

        public UserTagIndexer(
            IUserTagProvider userTagProvider, 
            ISearchUmbracoHelper searchUmbracoHelper,
            IIndexContext<SearchableTag> indexContext, 
            ISearchRepository<SearchableTag> searchRepository)
        {
            _userTagProvider = userTagProvider;
            _searchUmbracoHelper = searchUmbracoHelper;
            _indexContext = indexContext;
            _searchRepository = searchRepository;
        }

        public async Task<bool> RebuildIndex()
        {
            try
            {
                var tags = _userTagProvider.GetAll();
                var searchableTags = tags.Select(Map).ToList();
                await _indexContext.RecreateIndex();
                await _searchRepository.IndexAsync(searchableTags);

                return true;

                //return _indexerDiagnosticService.GetSuccessResult(typeof(UserTagsSearchIndexer).Name, searchableTags);
            }
            catch (Exception e)
            {
                return false;

                //return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(UserTagsSearchIndexer).Name);
            }
        }

        public Task<bool> Delete(IEnumerable<string> nodeIds)
        {
            return _searchRepository.DeleteAsync(nodeIds);
        }

        public Task Index(UserTag tag)
        {
            var searchableTag = Map(tag);
            return _searchRepository.IndexAsync(searchableTag);
        }

        public Task Delete(Guid id)
        {
            return _searchRepository.DeleteAsync(id.ToString());
        }

        private SearchableTag Map(UserTag tag)
        {
            var searchableTag = tag.Map<SearchableTag>();
            searchableTag.Url = _searchUmbracoHelper.GetSearchPage().Url.AddQueryParameter(tag.Text).ToLinkModel();
            return searchableTag;
        }
    }
}