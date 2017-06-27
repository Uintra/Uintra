using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var media = _umbracoHelper.TypedMedia(id);
            if (media == null)
            {
                return null;
            }

            var fileName = Path.GetFileName(media.Url);
            var extension = Path.GetExtension(fileName)?.Trim('.');

            if (!_settings.IndexingDocumentTypesKey.Contains(extension, StringComparison.OrdinalIgnoreCase))
            {
                return null;
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

            return result;
        }
    }
}
