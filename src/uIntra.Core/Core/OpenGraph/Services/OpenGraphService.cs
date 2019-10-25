using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Uintra.Core.Activity;
using Uintra.Core.OpenGraph.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using static Uintra.Core.OpenGraph.Constants.OpenGraphConstants;
using Uintra.Core.Extensions;
using Umbraco.Core;
using Uintra.Core.Media;

namespace Uintra.Core.OpenGraph.Services
{
    public class OpenGraphService : IOpenGraphService
    {
        private const string _queryStringIdKey = "id";

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly UmbracoHelper _umbracoHelper;

        private Uri RequestUrl { get { return HttpContext.Current.Request.Url; } }

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

            var uri = new UriBuilder(url).Uri;
            var content = _umbracoHelper.UmbracoContext.ContentCache.GetByRoute(uri.GetAbsolutePathDecoded());
            if (content == null) return null;

            if (content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetBulletinsDetailsPage()) ||
                content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetEventsDetailsPage()) ||
                content.DocumentTypeAlias.InvariantEquals(_documentTypeAliasProvider.GetNewsDetailsPage()))
            {
                return Guid.TryParse(HttpUtility.ParseQueryString(uri?.Query ?? "").Get(_queryStringIdKey), out var id) ?
                    GetOpenGraphObject(id, url) : null;
            }
            else
                return GetOpenGraphObject(content, url); ;
        }

        public virtual OpenGraphObject GetOpenGraphObject(IPublishedContent content, string defaultUrl = null)
        {
            var obj = GetDefaultObject(defaultUrl);
            if (!content.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetContentPage()))
            {
                obj.Title = content.Name;
                return obj;
            }

            var media = content.GetPropertyValue<IPublishedContent>(Properties.ImageAlias);
            obj.Title = content.HasValue(Properties.TitleAlias) ?
                content.GetPropertyValue<string>(Properties.TitleAlias) : content.Name;
            obj.Description = content.GetPropertyValue<string>(Properties.DescriptionAlias);
            obj.Image = GetAbsoluteImageUrl(media);
            obj.MediaId = media?.Id;
            return obj;
        }

        public virtual OpenGraphObject GetOpenGraphObject(Guid activityId, string defaultUrl = null)
        {
            var obj = GetDefaultObject(defaultUrl);

            var intranetActivityService = (IIntranetActivityService<IIntranetActivity>)_activitiesServiceFactory
                .GetService<IIntranetActivityService>(activityId);
            var currentActivity = intranetActivityService?.Get(activityId);
            if (currentActivity == null)
                return obj;

            obj.Title = currentActivity.Title.IfNullOrWhiteSpace("Bulletin");
            obj.Description = Extensions.StringExtensions.StripHtml(currentActivity.Description).TrimByWordEnd(100);

            if (currentActivity.MediaIds.Any())
            {
                foreach (var mediaId in currentActivity.MediaIds)
                {
                    var media = _umbracoHelper.TypedMedia(mediaId);
                    if (media.GetMediaType().Equals(MediaTypeEnum.Image))
                    {
                        obj.MediaId = mediaId;
                        obj.Image = GetAbsoluteImageUrl(media);
                        break;
                    }
                }
            }

            return obj;
        }

        protected virtual OpenGraphObject GetDefaultObject(string defaultUrl = null)
        {
            var obj = new OpenGraphObject()
            {
                SiteName = RequestUrl.Host,
                Type = "website",
                Url = defaultUrl.IsNullOrWhiteSpace() ? RequestUrl.AbsoluteUri : defaultUrl
            };

            return obj;
        }

        private string GetAbsoluteImageUrl(IPublishedContent media)
        {
            if (media == null)
                return null;
            return $"{RequestUrl.GetLeftPart(UriPartial.Authority)}{media.Url.TrimEnd('/')}";
        }
    }
}
