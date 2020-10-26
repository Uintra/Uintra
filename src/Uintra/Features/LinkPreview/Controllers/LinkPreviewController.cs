using Compent.LinkPreview.HttpClient;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra.Features.LinkPreview.Configurations;
using Uintra.Features.LinkPreview.Mappers;
using Uintra.Features.LinkPreview.Models;
using Uintra.Features.LinkPreview.Providers.Contracts;
using Uintra.Features.LinkPreview.Sql;
using Uintra.Features.OpenGraph.Models;
using Uintra.Features.OpenGraph.Services.Contracts;
using Uintra.Infrastructure.Extensions;
using Uintra.Persistence.Sql;

namespace Uintra.Features.LinkPreview.Controllers
{
    public class LinkPreviewController : UBaselineApiController
    {
        private readonly ILinkPreviewClient _linkPreviewClient;
        private readonly ISqlRepository<int, LinkPreviewEntity> _previewRepository;
        private readonly ILinkPreviewConfigProvider _configProvider;
        private readonly IOpenGraphService _openGraphService;
        private readonly LinkPreviewModelMapper _linkPreviewModelMapper;

        public LinkPreviewController(
            ILinkPreviewClient linkPreviewClient,
            ISqlRepository<int, LinkPreviewEntity> previewRepository,
            LinkPreviewModelMapper linkPreviewModelMapper,
            ILinkPreviewConfigProvider configProvider,
            IOpenGraphService openGraphService)
        {
            _linkPreviewClient = linkPreviewClient;
            _previewRepository = previewRepository;
            _linkPreviewModelMapper = linkPreviewModelMapper;
            _configProvider = configProvider;
            _openGraphService = openGraphService;
        }

        [HttpGet]
        public virtual async Task<LinkPreviewModel> Preview(string url)
        {
            var openGraphObject = _openGraphService.GetOpenGraphObject(url);

            if (openGraphObject != null)
            {
                var localRequestEntity = Map(openGraphObject);

                _previewRepository.Add(localRequestEntity);

                return _linkPreviewModelMapper.MapPreview(localRequestEntity);
            }

            var result = await _linkPreviewClient.GetLinkPreview(url);

            if (!result.IsSuccess) return null;

            var entity = Map(result.Preview, url);

            _previewRepository.Add(entity);

            var linkPreview = _linkPreviewModelMapper.MapPreview(entity);

            return linkPreview;
        }

        protected virtual LinkPreviewEntity Map(OpenGraphObject graph) =>
            new LinkPreviewEntity
            {
                OgDescription = graph.Description,
                Title = graph.Title,
                Uri = graph.Url,
                MediaId = graph.MediaId
            };

        protected virtual LinkPreviewEntity Map(Compent.LinkPreview.HttpClient.LinkPreview model, string url)
        {
            var entity = model.Map<LinkPreviewEntity>();

            entity.Uri = url;

            return entity;
        }

        [HttpGet]
        public virtual LinkDetectionConfig Config() =>
            _configProvider.Config;
    }
}