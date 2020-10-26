using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexers.Diagnostics;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Search.Configuration;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using File = System.IO.File;

namespace Uintra20.Core.Search.Indexers
{
    public class DocumentIndexer : IIndexer, IDocumentIndexer
    {
        private readonly IElasticDocumentIndex _documentIndex;
        private readonly ISearchApplicationSettings _settings;
        private readonly IMediaHelper _mediaHelper;
        private readonly IMediaService _mediaService;
        private readonly ILogger _logger;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService;

        public DocumentIndexer(IElasticDocumentIndex documentIndex,
            ISearchApplicationSettings settings,
            IMediaHelper mediaHelper,
            IMediaService mediaService, 
            ILogger logger,
            IIndexerDiagnosticService indexerDiagnosticService)
        {
            _documentIndex = documentIndex;
            _settings = settings;
            _mediaHelper = mediaHelper;
            _mediaService = mediaService;
            _logger = logger;
            _indexerDiagnosticService = indexerDiagnosticService;
        }

        public IndexedModelResult FillIndex()
        {
            try
            {
                var documentsToIndex = GetDocumentsForIndexing().ToList();
                Index(documentsToIndex);

                return _indexerDiagnosticService.GetSuccessResult(typeof(DocumentIndexer).Name, documentsToIndex);

            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(DocumentIndexer).Name);
            }
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

        public void Index(int id)
        {
            Index(id.ToEnumerable());
        }

        public void Index(IEnumerable<int> ids)
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
            _documentIndex.Index(documents);
        }

        public void DeleteFromIndex(int id)
        {
            DeleteFromIndex(id.ToEnumerable());
        }

        public void DeleteFromIndex(IEnumerable<int> ids)
        {
            var medias = _mediaService.GetByIds(ids);
            foreach (var media in medias)
            {
                if (IsAllowedForIndexing(media))
                {
                    media.SetValue(UmbracoAliases.Media.UseInSearchPropertyAlias, false);
                    _mediaService.Save(media);
                }
                _documentIndex.Delete(media.Id);
            }
        }

        private IEnumerable<SearchableDocument> GetSearchableDocument(int id)
        {
            var content =  Umbraco.Web.Composing.Current.UmbracoHelper.Media(id);
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

            bool isFileExtensionAllowedForIndex = _settings.IndexingDocumentTypesKey.Contains(extension, StringComparison.OrdinalIgnoreCase);


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
                    Id = content.Id,
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
