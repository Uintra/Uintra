using System.Web.Http;
using uIntra.Core.LinkPreview;
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
        public LinkPreview.LinkPreview Preview(string url)
        {
            return _linkPreviewService.GetLinkPreview(url);
        }
    }
}