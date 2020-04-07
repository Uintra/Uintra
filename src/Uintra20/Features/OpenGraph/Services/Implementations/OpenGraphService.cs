using System;
using System.Linq;
using System.Web;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.OpenGraph.Models;
using Uintra20.Features.OpenGraph.Services.Contracts;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using static Uintra20.Features.LinkPreview.Constants.OpenGraphConstants.Properties;
using StringExtensions = Uintra20.Infrastructure.Extensions.StringExtensions;

namespace Uintra20.Features.OpenGraph.Services.Implementations
{
    public class OpenGraphService : IOpenGraphService
    {
        private const string _queryStringIdKey = "id";

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly UmbracoHelper _umbracoHelper;

        private Uri RequestUrl => HttpContext.Current.Request.Url;

        public OpenGraphService(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IActivitiesServiceFactory activitiesServiceFactory,
            UmbracoHelper umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activitiesServiceFactory = activitiesServiceFactory;
            _umbracoHelper = umbracoHelper;
        }

        public virtual OpenGraphObject GetOpenGraphObject(string url)
        {
            if (url.IsNullOrWhiteSpace()) return null;

            return null;
            //var uri = new UriBuilder(url).Uri;
            //var content = HttpContext.Current.Request.Url.Host.Equals(uri.Host) ?
            //    _umbracoHelper.UmbracoContext.ContentCache.GetByRoute(uri.GetAbsolutePathDecoded()) : null;
            //if (content == null) return null;

            //if (content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetBulletinsDetailsPage()) ||
            //    content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetEventsDetailsPage()) ||
            //    content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetNewsDetailsPage()))
            //{
            //    return Guid.TryParse(HttpUtility.ParseQueryString(uri?.Query ?? string.Empty).Get(_queryStringIdKey), out var id) ?
            //        GetOpenGraphObject(id, url) : null;
            //}
            //else
            //    return GetOpenGraphObject(content, url);
        }

        public virtual OpenGraphObject GetOpenGraphObject(IPublishedContent content, string defaultUrl = null)
        {
            var defaultOpenGraph = GetDefaultObject(defaultUrl);
            //if (!content.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetContentPage()))
            //{
            //    obj.Title = content.Name;

            //    return obj;
            //}

            var media = content.Value<IPublishedContent>(ImageAlias);

            defaultOpenGraph.Title = content.HasValue(TitleAlias)
                ? content.Value<string>(TitleAlias)
                : content.Name;

            defaultOpenGraph.Description = content.Value<string>(DescriptionAlias);
            defaultOpenGraph.Image = GetAbsoluteImageUrl(media);
            defaultOpenGraph.MediaId = media?.Id;

            return defaultOpenGraph;
        }

        public virtual OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null)
        {
            var obj = GetDefaultObject(defaultUrl);

            var intranetActivityService = (IIntranetActivityService<IIntranetActivity>)_activitiesServiceFactory
                .GetService<IIntranetActivityService>(activityId);
            var currentActivity = intranetActivityService?.Get(activityId);
            if (currentActivity == null)
                return obj;

            obj.Title = currentActivity.Title.IfNullOrWhiteSpace("Social");
            obj.Description = StringExtensions.StripHtml(currentActivity.Description).TrimByWordEnd(100);

            if (currentActivity.MediaIds.Any())
            {
                foreach (var mediaId in currentActivity.MediaIds)
                {
                    //var media = _umbracoHelper.TypedMedia(mediaId);
                    //if (media.GetMediaType().Equals(MediaTypeEnum.Image))
                    //{
                    //    obj.MediaId = mediaId;
                    //    obj.Image = GetAbsoluteImageUrl(media);
                    //    break;
                    //}
                }
            }

            return obj;
        }

        protected virtual OpenGraphObject GetDefaultObject(string defaultUrl = null) =>
            new OpenGraphObject
            {
                SiteName = RequestUrl.Host,
                Type = "website",
                Url = defaultUrl.IsNullOrWhiteSpace() ? RequestUrl.AbsoluteUri : defaultUrl
            };

        private string GetAbsoluteImageUrl(IPublishedContent media)
        {
            if (media == null) return null;

            return $"{RequestUrl.GetLeftPart(UriPartial.Authority)}{media.Url.TrimEnd('/')}";
        }
    }
}