using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Web;

namespace Uintra20.Features.Activity.Helpers
{
    public class ActivityPageHelper : IActivityPageHelper
    {
        public Enum ActivityType { get; }

        private readonly IEnumerable<string> _activityXPath;
        private readonly string[] _baseXPath;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public ActivityPageHelper(Enum activityType, IEnumerable<string> baseXPath, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = documentTypeAliasProvider;
            _baseXPath = baseXPath.ToArray();
            ActivityType = activityType;

            _activityXPath = _baseXPath.Append(_aliasProvider.GetOverviewPage(ActivityType));
        }

        public string GetFeedUrl() => GetPageUrl(_baseXPath);

        public string GetOverviewPageUrl()
        {
            return GetPageUrl(_activityXPath);
        }

        public string GetDetailsPageUrl(Guid? activityId = null)
        {
            var xPath = _activityXPath.Append(_aliasProvider.GetDetailsPage(ActivityType));
            var detailsPageUrl = GetPageUrl(xPath);

            return activityId.HasValue ? detailsPageUrl.AddIdParameter(activityId) : detailsPageUrl;
        }

        public string GetCreatePageUrl() =>
            _aliasProvider
                .GetCreatePage(ActivityType)
                .Bind(createPage => createPage.Pipe(_activityXPath.Append).Pipe(GetPageUrl));

        public string GetEditPageUrl(Guid activityId)
        {
            var xPath = _activityXPath.Append(_aliasProvider.GetEditPage(ActivityType));
            return GetPageUrl(xPath).AddIdParameter(activityId);
        }

        private string GetPageUrl(IEnumerable<string> xPath)
        {
            return _umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(xPath.Where(x => x != null)))?.Url;
        }
    }
}