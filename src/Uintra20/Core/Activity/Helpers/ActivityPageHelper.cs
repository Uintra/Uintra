using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Activity.Helpers
{
    public class ActivityPageHelper : IActivityPageHelper//TODO: Needs research
    {
        public Enum ActivityType { get; }

        //private readonly IEnumerable<string> _activityXPath;
        //private readonly string[] _baseXPath;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly IPublishedContent _baseContent;

        public ActivityPageHelper(Enum activityType, IPublishedContent baseContent, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = documentTypeAliasProvider;
            //_baseXPath = baseXPath.ToArray();
            ActivityType = activityType;
            _baseContent = baseContent;

            //_activityXPath = _baseXPath.Append(_aliasProvider.GetOverviewPage(ActivityType));
        }

        public string GetFeedUrl() => GetPageUrl(_baseContent);

        //public string GetOverviewPageUrl()//TODO: Research overview page
        //{
        //    return GetPageUrl(_activityXPath);
        //}

        public string GetDetailsPageUrl(Guid? activityId = null)
        {
            var detailsPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetDetailsPage(ActivityType));
            var detailsPageUrl = GetPageUrl(detailsPageContent);

            return activityId.HasValue ? detailsPageUrl.AddIdParameter(activityId) : detailsPageUrl;
        }

        public string GetCreatePageUrl()
        {
            var createPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetCreatePage(ActivityType));

            return GetPageUrl(createPageContent);
        }

        public string GetEditPageUrl(Guid activityId)
        {
            var editPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetEditPage(ActivityType));

            return GetPageUrl(editPageContent)?.AddIdParameter(activityId);
        }

        private string GetPageUrl(IPublishedContent content)
        {
            return content?.Url(mode:UrlMode.Absolute);
        }
    }
}