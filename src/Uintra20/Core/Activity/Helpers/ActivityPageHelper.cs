using System;
using System.Linq;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;
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

        public UintraLinkModel GetFeedUrl() => GetPageUrl(_baseContent).ToLinkModel();

        //public string GetOverviewPageUrl()//TODO: Research overview page
        //{
        //    return GetPageUrl(_activityXPath);
        //}

        public UintraLinkModel GetDetailsPageUrl(Guid? activityId = null)
        {
            var detailsPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetDetailsPage(ActivityType));
            var detailsPageUrl = GetPageUrl(detailsPageContent);

            return activityId.HasValue ? detailsPageUrl.AddIdParameter(activityId).ToLinkModel() : detailsPageUrl.ToLinkModel();
        }

        public UintraLinkModel GetCreatePageUrl()
        {
            var createPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetCreatePage(ActivityType));

            return GetPageUrl(createPageContent).ToLinkModel();
        }

        public UintraLinkModel GetEditPageUrl(Guid activityId)
        {
            var editPageContent = _baseContent.Children.FirstOrDefault(x => x.ContentType.Alias == _aliasProvider.GetEditPage(ActivityType));

            return GetPageUrl(editPageContent)?.AddIdParameter(activityId).ToLinkModel();
        }

        private string GetPageUrl(IPublishedContent content)
        {
            return content?.Url(mode:UrlMode.Absolute);
        }
    }
}