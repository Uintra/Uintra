using System.Collections.Generic;
using Extensions;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Core.Activity
{
    class ActivityPageHelper : IActivityPageHelper
    {
        public IIntranetType ActivityType { get; }

        private readonly IEnumerable<string> _activityXPath;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public ActivityPageHelper(IIntranetType activityType, IEnumerable<string> baseXPath, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _aliasProvider = documentTypeAliasProvider;
            ActivityType = activityType;

            _activityXPath = baseXPath.Append(_aliasProvider.GetOverviewPage(ActivityType));
        }

        public string GetOverviewPageUrl()
        {
            return GetPageUrl(_activityXPath);
        }

        public string GetDetailsPageUrl()
        {
            var xPath = _activityXPath.Append(_aliasProvider.GetDetailsPage(ActivityType));
            return GetPageUrl(xPath);
        }

        public string GetCreatePageUrl() =>
            _aliasProvider
                .GetCreatePage(ActivityType)
                .Bind(createPage => createPage.Map(_activityXPath.Append).Map(GetPageUrl));

        public string GetEditPageUrl()
        {
            var xPath = _activityXPath.Append(_aliasProvider.GetEditPage(ActivityType));
            return GetPageUrl(xPath);
        }

        private string GetPageUrl(IEnumerable<string> xPath)
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(xPath))?.Url;
        }
    }
}