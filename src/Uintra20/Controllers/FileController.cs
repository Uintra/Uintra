using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra20.Core.Controls.FileUpload;
using Uintra20.Infrastructure.Caching;
using Umbraco.Web.WebApi;

namespace Uintra20.Controllers
{
    public class FileController : UmbracoApiController
    {
        private const int MaxUploadCount = 10;

        private readonly ICacheService cacheService;

        public FileController(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var idList = new List<Guid>(MaxUploadCount);

            foreach (var file in provider.Contents.Take(MaxUploadCount))
            {
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();

                var tempFile = new TempFile
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    FileBytes = buffer
                };

                cacheService.GetOrSet(tempFile.Id.ToString(), () => tempFile, DateTimeOffset.Now.AddDays(1));

                idList.Add(tempFile.Id);
            }

            var result = string.Join(",", idList);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}