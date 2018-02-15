using Compent.LinkPreview.HttpClient;
using Uintra.Core;
using Uintra.Core.LinkPreview;
using Uintra.Core.LinkPreview.Sql;
using Uintra.Core.Persistence;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class LinkPreviewController : LinkPreviewControllerBase
    {
        public LinkPreviewController(ILinkPreviewService linkPreviewService,
            ILinkPreviewConfigProvider configProvider,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper) 
            : base(linkPreviewService, configProvider, previewRepository, linkPreviewModelMapper)
        {
        }
    }
}