using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using uCommunity.Core.App_Plugins.Core.Caching;
using Umbraco.Web.WebApi;

namespace uCommunity.Core.App_Plugins.Core.Controls.FileUpload.Core
{
    public class FileController : UmbracoApiController
    {
        private readonly IMemoryCacheService _memoryCacheService;

        public FileController(IMemoryCacheService memoryCacheService)
        {
            _memoryCacheService = memoryCacheService;
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

            _memoryCacheService.GetOrSet(result.Id.ToString(), () => result, DateTimeOffset.Now.AddDays(1));

            return Request.CreateResponse(HttpStatusCode.OK, result.Id);
        }
    }
}