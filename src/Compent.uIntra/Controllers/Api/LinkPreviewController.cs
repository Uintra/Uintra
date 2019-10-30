using Compent.LinkPreview.Core.Images;
using Compent.LinkPreview.HttpClient;
using Uintra.Core;
using Uintra.Core.LinkPreview;
using Uintra.Core.LinkPreview.Sql;
using Uintra.Core.OpenGraph.Services;
using Uintra.Core.Persistence;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class LinkPreviewController : LinkPreviewControllerBase
    {
        public LinkPreviewController(
            ILinkPreviewClient linkPreviewClient,
            ILinkPreviewConfigProvider configProvider,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper,
            IOpenGraphService openGraphService
            ) 
            : base(linkPreviewClient, configProvider, previewRepository, linkPreviewModelMapper, openGraphService)
        {
        }
    }
}