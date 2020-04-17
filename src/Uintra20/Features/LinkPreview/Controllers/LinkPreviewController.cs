using Compent.LinkPreview.HttpClient;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Features.LinkPreview.Configurations;
using Uintra20.Features.LinkPreview.Mappers;
using Uintra20.Features.LinkPreview.Providers.Contracts;
using Uintra20.Features.LinkPreview.Sql;
using Uintra20.Features.OpenGraph.Models;
using Uintra20.Features.OpenGraph.Services.Contracts;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.LinkPreview.Controllers
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
        public virtual async Task<Models.LinkPreview> Preview(string url)
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

            linkPreview.Url = linkPreview.Uri.AbsoluteUri;
            
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