using uIntra.Core.LinkPreview.Services;
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