using System.Threading.Tasks;
using Compent.LinkPreview.HttpClient;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.LinkPreview.Sql;
using Uintra.Core.Persistence;
using Umbraco.Web.WebApi;

namespace Uintra.Core.Web
{
    public abstract class LinkPreviewControllerBase : UmbracoApiController
    {
        private readonly ILinkPreviewClient _linkPreviewClient;
        private readonly ILinkPreviewConfigProvider _configProvider;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        protected LinkPreviewControllerBase(
            ILinkPreviewClient linkPreviewClient,
            ILinkPreviewConfigProvider configProvider,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper)
        {
            _linkPreviewClient = linkPreviewClient;
            _configProvider = configProvider;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
        }

        [System.Web.Http.HttpGet]
        public virtual async Task<LinkPreview.LinkPreview> Preview(string url)
        {
            var result = await _linkPreviewClient.GetLinkPreview(url);
            if (!result.IsSuccess) return null;

            var entity = Map(result.Preview, url);
            _previewRepository.Add(entity);

            var model = _linkPreviewModelMapper.MapPreview(entity);
            return model;
        }

        protected virtual LinkPreviewEntity Map(Compent.LinkPreview.HttpClient.LinkPreview model, string url)
        {
            var entity = model.Map<LinkPreviewEntity>();
            entity.Uri = url;
            return entity;
        }

        [System.Web.Http.HttpGet]
        public virtual LinkDetectionConfig Config()
        {
            return _configProvider.Config;
        }
    }
}