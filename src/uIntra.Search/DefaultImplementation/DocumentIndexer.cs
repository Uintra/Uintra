using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using uIntra.Core.Extentions;
using uIntra.Search.Core;
using Umbraco.Web;

namespace uIntra.Search
{
    public class DocumentIndexer : IIndexer, IDocumentIndexer
    {
        private readonly IElasticDocumentIndex _documentIndex;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISearchApplicationSettings _settings;

        public DocumentIndexer(IElasticDocumentIndex documentIndex, UmbracoHelper umbracoHelper, ISearchApplicationSettings settings)
        {
            _documentIndex = documentIndex;
            _umbracoHelper = umbracoHelper;
            _settings = settings;
        }

        public void FillIndex()
        {
            //  throw new System.NotImplementedException();
        }

        public void Index(int id)
        {
            var media = _umbracoHelper.TypedMedia(id);
            if (media == null)
            {
                return;
            }

            var fileName = Path.GetFileName(media.Url);
            var extension = Path.GetExtension(fileName)?.Trim('.');

            if (!_settings.IndexingDocumentTypesKey.Contains(extension, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var physicalPath = HostingEnvironment.MapPath(media.Url);
            var base64File = Convert.ToBase64String(File.ReadAllBytes(physicalPath));

            var result = new SearchableDocument
            {
                Id = media.Id,
                Title = fileName,
                Url = media.Url,
                Data = base64File,
                Type = SearchableType.Document
            };

            _documentIndex.Index(result);
        }

        public void Index(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                Index(id);
            }
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
    }
}
