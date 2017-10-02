using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Groups
{
    public class GroupContentHelper : IGroupContentHelper
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IEnumerable<string> _overviewXPath;

        public GroupContentHelper(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _umbracoHelper = umbracoHelper;

            _overviewXPath = new[] { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage() };
        }

        public IPublishedContent GetGroupRoomPage() => 
            _documentTypeAliasProvider.GetGroupRoomPage()
                .Map(_overviewXPath.Append)
                .Map(GetPage);

        public IPublishedContent GetCreateGroupPage() =>
            _documentTypeAliasProvider.GetGroupCreatePage()
                .Map(_overviewXPath.Append)
                .Map(GetPage);

        public IPublishedContent GetOverviewPage() => 
            GetPage(_overviewXPath);

        public IPublishedContent GetEditPage() =>
            _documentTypeAliasProvider.GetGroupEditPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetPage);

        public IPublishedContent GetMyGroupsOverviewPage() => 
            _documentTypeAliasProvider
                .GetGroupMyGroupsOverviewPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetPage);

        public IPublishedContent GetDeactivatedGroupPage() => 
            _documentTypeAliasProvider
                .GetGroupDeactivatedPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetPage);

        private IEnumerable<string> GetAtGroupRoomXPath(string pageAlias) => 
            _overviewXPath
                .Append(_documentTypeAliasProvider.GetGroupRoomPage())
                .Append(pageAlias);

        private IPublishedContent GetPage(IEnumerable<string> xPath) => 
            _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(xPath));
    }
}