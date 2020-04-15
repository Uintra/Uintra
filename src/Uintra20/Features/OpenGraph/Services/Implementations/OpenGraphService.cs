using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Media;
using UBaseline.Core.Node;
using UBaseline.Shared.MetaData;
using UBaseline.Shared.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.OpenGraph.Models;
using Uintra20.Features.OpenGraph.Services.Contracts;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using StringExtensions = Uintra20.Infrastructure.Extensions.StringExtensions;

namespace Uintra20.Features.OpenGraph.Services.Implementations
{
    public class OpenGraphService : IOpenGraphService
    {
        private const string _queryStringIdKey = "id";

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INodeModelService _nodeModelService;
        private readonly IMediaProvider _mediaProvider;
        

        private Uri RequestUrl => HttpContext.Current.Request.Url;

        public OpenGraphService(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IActivitiesServiceFactory activitiesServiceFactory,
            INodeModelService nodeModelService,
            IMediaService mediaService, 
            IMediaHelper mediaHelper, 
            IMediaProvider mediaProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activitiesServiceFactory = activitiesServiceFactory;
            _nodeModelService = nodeModelService;
            _mediaProvider = mediaProvider;
        }

        public virtual OpenGraphObject GetOpenGraphObject(string url)
        {
            if (url.IsNullOrWhiteSpace()) return null;

            var uri = new UriBuilder(url).Uri;

            var content = HttpContext.Current.Request.Url.Host.Equals(uri.Host)
                ? _nodeModelService.GetByUrl(uri, null)
                : null;

            if (content == null) return null;

            if (IsActivityDetailsPage(content))
            {
                var query = HttpUtility.ParseQueryString(uri.Query);

                var tryParse = Guid.TryParse(query.Get(_queryStringIdKey), out var id);

                return tryParse
                    ? GetOpenGraphObject(id, url)
                    : null;
            }

            return GetOpenGraphObject(content, url);
        }

        private bool IsActivityDetailsPage(INodeModel content) =>
            content.ContentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetBulletinsDetailsPage()) ||
            content.ContentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetEventsDetailsPage()) ||
            content.ContentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetNewsDetailsPage());

        public virtual OpenGraphObject GetOpenGraphObject(INodeModel nodeModel, string defaultUrl = null)
        {
            var graph = GetDefaultObject(defaultUrl);

            if (nodeModel is IMetaDataComposition metaData)
            {
                graph.Title = metaData.MetaData.MetaTitle;
                graph.Description = metaData.MetaData.MetaDescription;
                graph.MediaId = metaData.MetaData.SocialImage.DataTypeId;
                graph.Image = _mediaProvider.GetById(metaData.MetaData.SocialImage.Value.MediaId).Url;
            }

            return graph;
        }

        public virtual OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null)
        {
            var defaultGraph = GetDefaultObject(defaultUrl);

            var intranetActivityService = (IIntranetActivityService<IIntranetActivity>)
                _activitiesServiceFactory.GetService<IIntranetActivityService>(activityId);

            var currentActivity = intranetActivityService.Get(activityId);

            if (currentActivity == null) return defaultGraph;

            defaultGraph.Title = currentActivity.Title.IfNullOrWhiteSpace("Social");
            defaultGraph.Description = StringExtensions.StripHtml(currentActivity.Description).TrimByWordEnd(100);

            if (currentActivity.MediaIds.Any())
            {
                foreach (var mediaId in currentActivity.MediaIds)
                {
                    var mediaModel = _mediaProvider.GetById(mediaId) as IMetaDataComposition;

                    defaultGraph.MediaId = mediaId;
                    defaultGraph.Image = _mediaProvider.GetById(mediaModel.MetaData.SocialImage.Value.MediaId).Url;
                }
            }

            return defaultGraph;
        }

        protected virtual OpenGraphObject GetDefaultObject(string url = null) =>
            new OpenGraphObject
            {
                SiteName = RequestUrl.Host,
                Type = "website",
                Url = url.IsNullOrWhiteSpace()
                    ? RequestUrl.AbsoluteUri
                    : url
            };
    }
}