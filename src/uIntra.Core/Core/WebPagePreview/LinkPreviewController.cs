using Umbraco.Web.WebApi;

namespace uIntra.Core.WebPagePreview
{
    public class LinkPreviewController: UmbracoApiController
    {
        private readonly ILinkPreviewService _linkPreviewService;

        public LinkPreviewController(ILinkPreviewService linkPreviewService)
        {
            _linkPreviewService = linkPreviewService;
        }

        [System.Web.Http.HttpGet]
        public byte[] GetHtmlPreview(string url)
        {
            return _linkPreviewService.GetHtmlPreviewByteArray(url);
        }

    }
}
