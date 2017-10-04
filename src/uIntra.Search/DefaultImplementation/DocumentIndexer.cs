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
using Umbraco.Web;
using File = System.IO.File;

namespace uIntra.Search
{
    public class DocumentIndexer : IIndexer, IDocumentIndexer
    {
        private readonly IElasticDocumentIndex _documentIndex;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchApplicationSettings _settings;
        private readonly IMediaHelper _mediaHelper;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMediaFolderTypeProvider _mediaFolderTypeProvider;

        public DocumentIndexer(IElasticDocumentIndex documentIndex,
            UmbracoHelper umbracoHelper, 
            ISearchApplicationSettings settings, 
            IMediaHelper mediaHelper,
            IExceptionLogger exceptionLogger, 
            IMediaFolderTypeProvider mediaFolderTypeProvider)
        {
            _documentIndex = documentIndex;
            _umbracoHelper = umbracoHelper;
            _settings = settings;
            _mediaHelper = mediaHelper;
            _exceptionLogger = exceptionLogger;
            _mediaFolderTypeProvider = mediaFolderTypeProvider;
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
            return media.GetPropertyValue<bool>("useInSearch");
        }

        public void Index(int id)
        {
            Index(id.ToEnumerableOfOne());
        }

        public void Index(IEnumerable<int> ids)
        {
            var documents = ids.Select(GetSearchableDocument).Where(el => el != null).ToList();
            _documentIndex.Index(documents);
        }

        public void DeleteFromIndex(int id)
        {
            _documentIndex.Delete(id);
        }

        public void DeleteFromIndex(IEnumerable<int> ids)
        {
            foreach (var id in ids)
                _documentIndex.Delete(id);

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
