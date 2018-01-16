using Compent.LinkPreview.Client;
using uIntra.Core.LinkPreview;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class LinkPreviewApiController : LinkPreviewApiControllerBase
    {
        public LinkPreviewApiController(ILinkPreviewService linkPreviewService) : base(linkPreviewService)
        {
        }
    }
}