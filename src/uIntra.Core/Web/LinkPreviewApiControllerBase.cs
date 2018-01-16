using System.Threading.Tasks;
using System.Web.Http;
using Compent.LinkPreview.Client;
using Umbraco.Web.WebApi;

namespace uIntra.Core.Web
{
    public abstract class LinkPreviewApiControllerBase : UmbracoApiController
    {
        private readonly ILinkPreviewService _linkPreviewService;

        protected LinkPreviewApiControllerBase(ILinkPreviewService linkPreviewService)
        {
            _linkPreviewService = linkPreviewService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<Compent.LinkPreview.Client.LinkPreview> Preview(string url)
        {
            return await _linkPreviewService.GetLinkPreview(url);
        }
    }
}