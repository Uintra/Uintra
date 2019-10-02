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

namespace Uintra.Core.OpenGraph.Services
{
    public class OpenGraphService : IOpenGraphService
    {
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

        public virtual OpenGraphObject GetOpenGraphObject(IPublishedContent content)
        {
            if (!content.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetContentPage()))
                return null;

            var obj = GetDefaultObject();

            obj.Title = content.HasValue(Properties.TitleAlias) ?
                content.GetPropertyValue<string>(Properties.TitleAlias) : content.Name;
            obj.Description = content.GetPropertyValue<string>(Properties.DescriptionAlias);
            obj.Image = content.HasValue(Properties.ImageAlias) ?
                GetAbsoluteImageUrl(content.GetPropertyValue<IPublishedContent>(Properties.ImageAlias)) : null;
            return obj;
        }

        public virtual OpenGraphObject GetOpenGraphObject(Guid activityId)
        {
            var obj = GetDefaultObject();

            var intranetActivityService = (IIntranetActivityService<IIntranetActivity>)_activitiesServiceFactory
                .GetService<IIntranetActivityService>(activityId);
            var currentActivity = intranetActivityService?.Get(activityId);
            if (currentActivity == null)
                return obj;

            obj.Title = currentActivity.Title;
            obj.Description = currentActivity.Description.StripHtml().TrimByWordEnd(100);

            if (currentActivity.MediaIds.Any())
                obj.Image = GetAbsoluteImageUrl(_umbracoHelper.TypedMedia(currentActivity.MediaIds.First()));

            return obj;
        }

        protected virtual OpenGraphObject GetDefaultObject()
        {
            var obj = new OpenGraphObject()
            {
                SiteName = RequestUrl.Host,
                Type = "website",
                Url = RequestUrl.AbsoluteUri
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
