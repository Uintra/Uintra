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

        public DocumentIndexer(IElasticDocumentIndex documentIndex, UmbracoHelper umbracoHelper)
        {
            _documentIndex = documentIndex;
            _umbracoHelper = umbracoHelper;
        }

        public void FillIndex()
        {
            //  throw new System.NotImplementedException();
        }

        public void Index(int id)
        {
            var supportDocTypes = "doc,docx,pdf,ppt,pptx,odf".Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var media = _umbracoHelper.TypedMedia(id);
            var physicalPath = HostingEnvironment.MapPath(media.Url);
            var fileName = Path.GetFileName(media.Url);
            var base64File = Convert.ToBase64String(File.ReadAllBytes(physicalPath));
            var contentType = Path.GetExtension(fileName)?.Trim('.');

            if (!supportDocTypes.Contains(contentType, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

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
