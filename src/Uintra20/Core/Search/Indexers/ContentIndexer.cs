using System;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Helpers;
using Uintra20.Core.Search.Indexers.Diagnostics;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Search.Web;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Uintra20.Core.Search.Indexers
{
    public class ContentIndexer : IIndexer, IContentIndexer
    {
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly IElasticContentIndex _contentIndex;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService;
        private readonly IContentService _contentService;

        public ContentIndexer(
            ISearchUmbracoHelper searchUmbracoHelper,
            IElasticContentIndex contentIndex,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IIndexerDiagnosticService indexerDiagnosticService,
            IContentService contentService)
        {
            _searchUmbracoHelper = searchUmbracoHelper;
            _contentIndex = contentIndex;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _indexerDiagnosticService = indexerDiagnosticService;
            _contentService = contentService;
        }

        public IndexedModelResult FillIndex()
        {
            try
            {
                var homePage = Umbraco.Web.Composing.Current.UmbracoHelper .ContentAtRoot().First(pc =>
                    pc.ContentType.Alias.Equals(_documentTypeAliasProvider.GetHomePage()));
                var contentPages = homePage.DescendantsOfType(_documentTypeAliasProvider.GetArticlePage());

                var searchableContents = contentPages
                    .Where(pc => _searchUmbracoHelper.IsSearchable(pc))
                    .Select(_searchUmbracoHelper.GetContent);
                _contentIndex.Index(searchableContents);

                return _indexerDiagnosticService.GetSuccessResult(typeof(ContentIndexer).Name, searchableContents);
            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(ContentIndexer).Name);
            }
        }

        public void FillIndex(int id)
        {
            var publishedContent = Umbraco.Web.Composing.Current.UmbracoHelper.Content(id);
            if (!IsArticlePage(publishedContent))
            {
                return;
            }
            
            var isSearchable = _searchUmbracoHelper.IsSearchable(publishedContent);
            if (isSearchable)
            {
                _contentIndex.Delete(publishedContent.Id);
                _contentIndex.Index(_searchUmbracoHelper.GetContent(publishedContent));
            }
            else
            {
                _contentIndex.Delete(publishedContent.Id);
            }
        }

        public void DeleteFromIndex(int id)
        {
            var content = _contentService.GetById(id);
            if (!IsArticlePage(content))
            {
                return;
            }

            _contentIndex.Delete(id);
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