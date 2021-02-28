using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Compent.Extensions;
using Compent.Shared.Search.Contract;
using UBaseline.Search.Core;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Repository;
using Uintra.Features.Media.Helpers;
using Uintra.Infrastructure.Constants;
using Uintra.Infrastructure.Extensions;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using File = System.IO.File;

namespace Uintra.Core.Search.Indexers
{
    public class DocumentIndexer : IDocumentIndexer
    {
        public Type Type { get; } = typeof(SearchableDocument);

        private readonly IIndexContext<SearchableDocument> _indexContext;
        private readonly IUintraSearchRepository<SearchableDocument> _searchRepository;
        private readonly IMediaHelper _mediaHelper;
        private readonly IMediaService _mediaService;
        private readonly ISearchSettings _searchSettings;
        private readonly ILogger _logger;

        public DocumentIndexer(
            IIndexContext<SearchableDocument> indexContext,
            IUintraSearchRepository<SearchableDocument> searchRepository, 
            IMediaHelper mediaHelper, 
            IMediaService mediaService, 
            ISearchSettings searchSettings,
            ILogger logger)
        {
            _indexContext = indexContext;
            _searchRepository = searchRepository;
            _mediaHelper = mediaHelper;
            _mediaService = mediaService;
            _searchSettings = searchSettings;
            _logger = logger;
        }

        public async Task<bool> RebuildIndex()
        {
            try
            {
                var documentsToIndex = GetDocumentsForIndexing().ToList();
                var rc = await _indexContext.RecreateIndex();
                var r = await Index(documentsToIndex);

                return true;
                //return _indexerDiagnosticService.GetSuccessResult(typeof(DocumentIndexer).Name, documentsToIndex);

            }
            catch (Exception e)
            {
                return false;

                //return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(DocumentIndexer).Name);
            }
        }

        public Task<bool> Delete(IEnumerable<string> nodeIds)
        {
            throw new NotImplementedException();
        }
        
        public Task<int> Index(int id)
        {
            return Index(id.ToEnumerable());
        }

        public Task<int> Index(IEnumerable<int> ids)
        {
            var medias = _mediaService.GetByIds(ids);
            var documents = new List<SearchableDocument>();

            foreach (var media in medias)
            {
                var document = GetSearchableDocument(media.Id);
                if (!document.Any()) continue;

                if (!IsAllowedForIndexing(media))
                {
                    media.SetValue(UmbracoAliases.Media.UseInSearchPropertyAlias, true);
                    _mediaService.Save(media);
                }

                documents.AddRange(document);
            }

            return _searchRepository.IndexAsync(documents);

            //_documentIndex.Index(documents);
        }

        public Task<bool> DeleteFromIndex(int id)
        {
            return DeleteFromIndex(id.ToEnumerable());
        }

        public Task<bool> DeleteFromIndex(IEnumerable<int> ids)
        {
            var medias = _mediaService.GetByIds(ids);
            foreach (var media in medias)
            {
                if (IsAllowedForIndexing(media))
                {
                    media.SetValue(UmbracoAliases.Media.UseInSearchPropertyAlias, false);
                    _mediaService.Save(media);
                }
                
                //_documentIndex.Delete(media.Id);
            }

            var idsToDelete = ids.Select(id => id.ToString());
            return _searchRepository.DeleteAsync(idsToDelete);
        }
        private IEnumerable<int> GetDocumentsForIndexing()
        {
            var medias = Umbraco.Web.Composing.Current.UmbracoHelper
                .MediaAtRoot()
                .SelectMany(m => m.DescendantsOrSelf());

            var result = medias
                .Where(c => IsAllowedForIndexing(c) && !_mediaHelper.IsMediaDeleted(c))
                .Select(m => m.Id);

            return result.ToList();
        }

        private bool IsAllowedForIndexing(IPublishedContent media)
        {
            return media.HasProperty(UmbracoAliases.Media.UseInSearchPropertyAlias) && media.GetProperty(UmbracoAliases.Media.UseInSearchPropertyAlias).Value<bool>();
        }

        private bool IsAllowedForIndexing(IMedia media)
        {
            return media.HasProperty(UmbracoAliases.Media.UseInSearchPropertyAlias) && media.GetValue<bool>(UmbracoAliases.Media.UseInSearchPropertyAlias);
        }

        private IEnumerable<SearchableDocument> GetSearchableDocument(int id)
        {
            var content = Umbraco.Web.Composing.Current.UmbracoHelper.Media(id);
            if (content == null)
            {
                return Enumerable.Empty<SearchableDocument>();
            }

            return GetSearchableDocument(content);
        }

        private IEnumerable<SearchableDocument> GetSearchableDocument(IPublishedContent content)
        {
            var fileName = Path.GetFileName(content.Url);
            var extension = Path.GetExtension(fileName)?.Trim('.');

            var isFileExtensionAllowedForIndex = _searchSettings.IndexingDocumentTypes.Contains(extension, StringComparison.OrdinalIgnoreCase);
            
            if (!content.Url.IsNullOrEmpty())
            {
                var physicalPath = HostingEnvironment.MapPath(content.Url);

                if (!File.Exists(physicalPath))
                {
                    _logger.Error<DocumentIndexer>(new FileNotFoundException($"Could not find file \"{physicalPath}\""));
                    return Enumerable.Empty<SearchableDocument>();
                }

                var base64File = isFileExtensionAllowedForIndex ? Convert.ToBase64String(File.ReadAllBytes(physicalPath)) : string.Empty;
                var result = new SearchableDocument
                {
                    Id = content.Id.ToString(),
                    Title = fileName,
                    Url = content.Url.ToLinkModel(),
                    Data = base64File,
                    Type = SearchableTypeEnum.Document.ToInt()
                };
                return result.ToEnumerable();
            }

            return Enumerable.Empty<SearchableDocument>();
        }
    }
}