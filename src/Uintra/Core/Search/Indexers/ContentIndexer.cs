using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Compent.Shared.Search.Contract;
using UBaseline.Search.Core;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Infrastructure.Providers;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Uintra.Core.Search.Indexers
{
    public class ContentIndexer : IContentIndexer
    {
        public Type Type { get; } = typeof(SearchableContent);

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IIndexContext<SearchableContent> _indexContext;
        private readonly ISearchRepository<SearchableContent> _searchRepository;
        private readonly IContentService _contentService;

        public ContentIndexer(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            ISearchUmbracoHelper searchUmbracoHelper, 
            IIndexContext<SearchableContent> indexContext, 
            ISearchRepository<SearchableContent> searchRepository, 
            IContentService contentService)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _searchUmbracoHelper = searchUmbracoHelper;
            _indexContext = indexContext;
            _searchRepository = searchRepository;
            _contentService = contentService;
        }


        public async Task<bool> RebuildIndex()
        {
            try
            {
                var homePage = Umbraco.Web.Composing.Current.UmbracoHelper.ContentAtRoot().First(pc =>
                    pc.ContentType.Alias.Equals(_documentTypeAliasProvider.GetHomePage()));
                var contentPages = homePage.DescendantsOfType(_documentTypeAliasProvider.GetArticlePage());

                var searchableContents = contentPages
                    .Where(pc => _searchUmbracoHelper.IsSearchable(pc))
                    .Select(_searchUmbracoHelper.GetContent);

                await _indexContext.RecreateIndex();
                await _searchRepository.IndexAsync(searchableContents);
                //_contentIndex.Index(searchableContents);

                return true;

                //return _indexerDiagnosticService.GetSuccessResult(typeof(ContentIndexer).Name, searchableContents);
            }
            catch (Exception e)
            {
                return false;

                //return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(ContentIndexer).Name);
            }
        }

        public async Task Index(int id)
        {
            var publishedContent = Umbraco.Web.Composing.Current.UmbracoHelper.Content(id);
            if (!IsArticlePage(publishedContent))
            {
                return;
            }

            var isSearchable = _searchUmbracoHelper.IsSearchable(publishedContent);
            if (isSearchable)
            {
                await _searchRepository.DeleteAsync(publishedContent.Id.ToString());
                await _searchRepository.IndexAsync(_searchUmbracoHelper.GetContent(publishedContent));
            }
            else
            {
                await _searchRepository.DeleteAsync(publishedContent.Id.ToString());
            }
        }

        public Task<bool> Delete(IEnumerable<string> nodeIds)
        {
            //TODO: Search. Check ids type. Add methods by type
            var ids = nodeIds.Select(int.Parse);
            var content = _contentService.GetByIds(ids);

            var contentIdsToDelete = content
                .Where(IsArticlePage)
                .Select(c => c.Id.ToString());
            
            return _searchRepository.DeleteAsync(contentIdsToDelete);
        }

        public Task Delete(int id)
        {
            var content = _contentService.GetById(id);

            if(IsArticlePage(content))
                return _searchRepository.DeleteAsync(id.ToString());

            return Task.CompletedTask;
        }

        private bool IsArticlePage(IPublishedContent publishedContent)
        {
            if (publishedContent == null) return false;
            return CheckArticlePageType(publishedContent.ContentType.Alias);
        }

        private bool IsArticlePage(IContent content)
        {
            if (content == null) return false;
            return CheckArticlePageType(content.ContentType.Alias);
        }

        private bool CheckArticlePageType(string type)
        {
            return type == _documentTypeAliasProvider.GetArticlePage();
        }
    }
}