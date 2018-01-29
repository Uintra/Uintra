using Compent.LinkPreview.Client;
using uIntra.Core;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class LinkPreviewApiController : LinkPreviewApiControllerBase
    {
        public LinkPreviewApiController(ILinkPreviewService linkPreviewService, ILinkPreviewConfigProvider configProvider) : base(linkPreviewService, configProvider)
        {
        }
    }
}