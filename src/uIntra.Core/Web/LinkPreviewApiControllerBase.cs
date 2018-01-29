using System.Threading.Tasks;
using System.Web.Http;
using Compent.LinkPreview.Client;
using Umbraco.Web.WebApi;

namespace uIntra.Core.Web
{
    public abstract class LinkPreviewApiControllerBase : UmbracoApiController
    {
        private readonly ILinkPreviewService _linkPreviewService;
        private readonly ILinkPreviewConfigProvider _configProvider;

        protected LinkPreviewApiControllerBase(ILinkPreviewService linkPreviewService, ILinkPreviewConfigProvider configProvider)
        {
            _linkPreviewService = linkPreviewService;
            _configProvider = configProvider;
        }

        [HttpGet, AllowAnonymous]
        public async Task<Compent.LinkPreview.Client.LinkPreview> Preview(string url)
        {
            return await _linkPreviewService.GetLinkPreview(url);
        }

        [HttpGet, AllowAnonymous]
        public FooBananaConfig Config()
        {
            return _configProvider.Config;
        }
    }
}