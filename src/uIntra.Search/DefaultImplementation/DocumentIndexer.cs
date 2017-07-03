using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
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

        public DocumentIndexer(IElasticDocumentIndex documentIndex,
            UmbracoHelper umbracoHelper, 
            ISearchApplicationSettings settings, 
            IMediaHelper mediaHelper)
        {
            _documentIndex = documentIndex;
            _umbracoHelper = umbracoHelper;
            _settings = settings;
            _mediaHelper = mediaHelper;
        }

        public void FillIndex()
        {
            var mediaFolderTypes = Enum.GetValues(typeof(MediaFolderTypeEnum));
            foreach (MediaFolderTypeEnum folderType in mediaFolderTypes)
            {
                var mediaRootId = _mediaHelper.GetMediaFolderSettings(folderType).MediaRootId;
                if (!mediaRootId.HasValue)
                {
                    continue;
                }

                var mediaFolder = _umbracoHelper.TypedMedia(mediaRootId.Value);
                var documents = mediaFolder.Children.Select(GetSearchableDocument).Where(el => el != null).ToList();
                if (!documents.Any())
                {
                    continue;
                }

                _documentIndex.Index(documents);
            }
        }

        public void Index(int id)
        {
            var document = GetSearchableDocument(id);
            if (document == null)
            {
                return;
            }

            _documentIndex.Index(document);
        }

        public void Index(IEnumerable<int> ids)
        {
            var documents = ids.Select(GetSearchableDocument).Where(el => el != null).ToList();
            if (!documents.Any())
            {
                return;
            }

            _documentIndex.Index(documents);
        }

        public void DeleteFromIndex(int id)
        {
            _documentIndex.Delete(id);
        }

        public void DeleteFromIndex(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                _documentIndex.Delete(id);
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

            var physicalPath = HostingEnvironment.MapPath(content.Url);
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
