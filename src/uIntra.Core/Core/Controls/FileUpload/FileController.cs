using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra.Core.Caching;
using Umbraco.Web.WebApi;

namespace Uintra.Core.Controls.FileUpload
{
    public class FileController : UmbracoApiController
    {
        private readonly ICacheService cacheService;

        public FileController(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> UploadSingle()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.Single();
            var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
            var buffer = await file.ReadAsByteArrayAsync();

            var result = new TempFile
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                FileBytes = buffer
            };

            cacheService.GetOrSet(result.Id.ToString(), () => result, DateTimeOffset.Now.AddDays(1));

            return Request.CreateResponse(HttpStatusCode.OK, result.Id);
        }
    }
}