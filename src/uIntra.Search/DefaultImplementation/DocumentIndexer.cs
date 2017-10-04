using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using uIntra.Core.Exceptions;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using File = System.IO.File;

namespace uIntra.Search
{
    public class DocumentIndexer : IIndexer, IDocumentIndexer
    {
        public const string UseInSearchPropertyAlias = "useInSearch";
        private readonly IElasticDocumentIndex _documentIndex;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchApplicationSettings _settings;
        private readonly IMediaHelper _mediaHelper;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IContentService _contentService;

        public DocumentIndexer(IElasticDocumentIndex documentIndex,
            UmbracoHelper umbracoHelper, 
            ISearchApplicationSettings settings, 
            IMediaHelper mediaHelper,
            IExceptionLogger exceptionLogger,
            IContentService contentService)
        {
            _documentIndex = documentIndex;
            _umbracoHelper = umbracoHelper;
            _settings = settings;
            _mediaHelper = mediaHelper;
            _exceptionLogger = exceptionLogger;
            _contentService = contentService;
        }

        public void FillIndex()
        {
            var documentsToIndex = GetDocumentsForIndexing();
            _documentIndex.Index(documentsToIndex);
        }

        private IEnumerable<SearchableDocument> GetDocumentsForIndexing()
        {
            var medias = _umbracoHelper
                .TypedMediaAtRoot()
                .SelectMany(m => m.DescendantsOrSelf());

            var result = medias
                .Where(IsAllowedForIndexing)
                .Select(GetSearchableDocument);

            return result.ToList();
        }

        private bool IsAllowedForIndexing(IPublishedContent media)
        {
            return media.GetPropertyValue<bool>(UseInSearchPropertyAlias);
        }

        public void Index(int id)
        {
            Index(id.ToEnumerableOfOne());
        }

        public void Index(IEnumerable<int> ids)
        {
            var medias = _contentService.GetByIds(ids);
            var documents = new List<SearchableDocument>();

            foreach (var media in medias)
            {
                var document = GetSearchableDocument(media.Id);
                if (document == null) continue;
                media.SetValue(UseInSearchPropertyAlias, true);
                _contentService.SaveAndPublishWithStatus(media);
                documents.Add(document);
            }
            _documentIndex.Index(documents);
        }

        public void DeleteFromIndex(int id)
        {
            DeleteFromIndex(id.ToEnumerableOfOne());
        }

        public void DeleteFromIndex(IEnumerable<int> ids)
        {
            var medias = _contentService.GetByIds(ids);
            foreach (var media in medias)
            {
                media.SetValue(UseInSearchPropertyAlias, false);
                _contentService.SaveAndPublishWithStatus(media);
                _documentIndex.Delete(media.Id);
            }
        }

        private SearchableDocument GetSearchableDocument(int id)
        {
            var content = _umbracoHelper.TypedMedia(id);
            if (content == null)
            {
                return null;
            }

            return GetSearchableDocument(content);
        }

        private SearchableDocument GetSearchableDocument(IPublishedContent content)
        {
            if (_mediaHelper.IsMediaDeleted(content))
            {
                return null;
            }

            var fileName = Path.GetFileName(content.Url);
            var extension = Path.GetExtension(fileName)?.Trim('.');

            if (!_settings.IndexingDocumentTypesKey.Contains(extension, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }


            if (content.Url.IsNullOrEmpty())
            {
                return null;
            }

            var physicalPath = HostingEnvironment.MapPath(content.Url);

            if (!File.Exists(physicalPath))
            {
                _exceptionLogger.Log(new FileNotFoundException($"Could not find file \"{physicalPath}\""));
                return null;
            }

            var base64File = Convert.ToBase64String(File.ReadAllBytes(physicalPath));

            var result = new SearchableDocument
            {
                Id = content.Id,
                Title = fileName,
                Url = content.Url,
                Data = base64File,
                Type = SearchableTypeEnum.Document.ToInt()
            };

            return result;
        }
    }
}
