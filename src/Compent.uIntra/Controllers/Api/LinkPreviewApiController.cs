using Compent.LinkPreview.Client;
using uIntra.Core;
using uIntra.Core.LinkPreview;
using uIntra.Core.LinkPreview.Sql;
using uIntra.Core.Persistence;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers.Api
{
    public class LinkPreviewApiController : LinkPreviewApiControllerBase
    {
        public LinkPreviewApiController(ILinkPreviewService linkPreviewService,
            ILinkPreviewConfigProvider configProvider,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper) 
            : base(linkPreviewService, configProvider, previewRepository, linkPreviewModelMapper)
        {
        }
    }
}