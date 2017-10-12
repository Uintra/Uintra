using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Groups
{
    public class GroupContentProvider : ContentProviderBase, IGroupContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IEnumerable<string> _overviewXPath;

        public GroupContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper) 
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _overviewXPath = new[] { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage() };
        }

        public IPublishedContent GetGroupRoomPage() => 
            _documentTypeAliasProvider.GetGroupRoomPage()
                .Map(_overviewXPath.Append)
                .Map(GetContent);

        public IPublishedContent GetCreateGroupPage() =>
            _documentTypeAliasProvider.GetGroupCreatePage()
                .Map(_overviewXPath.Append)
                .Map(GetContent);

        public IPublishedContent GetOverviewPage() => 
            GetContent(_overviewXPath);

        public IPublishedContent GetEditPage() =>
            _documentTypeAliasProvider.GetGroupEditPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetContent);

        public IPublishedContent GetMyGroupsOverviewPage() => 
            _documentTypeAliasProvider
                .GetGroupMyGroupsOverviewPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetContent);

        public IPublishedContent GetDeactivatedGroupPage() => 
            _documentTypeAliasProvider
                .GetGroupDeactivatedPage()
                .Map(GetAtGroupRoomXPath)
                .Map(GetContent);

        private IEnumerable<string> GetAtGroupRoomXPath(string pageAlias) => 
            _overviewXPath
                .Append(_documentTypeAliasProvider.GetGroupRoomPage())
                .Append(pageAlias);
    }
}