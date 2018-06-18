using System.Collections.Generic;
using uIntra.CentralFeed.Providers;
using uIntra.Core;
using uIntra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Groups
{
    public class GroupContentProvider : FeedContentProviderBase, IGroupContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        protected override IEnumerable<string> OverviewXPath { get; }

        public GroupContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper) 
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            OverviewXPath = new[] { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage() };
        }

        public override IEnumerable<IPublishedContent> GetRelatedPages() => 
            GetGroupRoomPage().Children;

        public IPublishedContent GetGroupRoomPage() => 
            _documentTypeAliasProvider.GetGroupRoomPage()
                .Map(OverviewXPath.Append)
                .Map(GetContent);

        public IPublishedContent GetCreateGroupPage() =>
            _documentTypeAliasProvider.GetGroupCreatePage()
                .Map(OverviewXPath.Append)
                .Map(GetContent);

        public IPublishedContent GetEditPage() =>
            _documentTypeAliasProvider.GetGroupEditPage()
                .Map(GetXPathAtGroupRoom)
                .Map(GetContent);

        public IPublishedContent GetMyGroupsOverviewPage() => 
            _documentTypeAliasProvider
                .GetGroupMyGroupsOverviewPage()
                .Map(GetXPathAtGroupRoom)
                .Map(GetContent);

        public IPublishedContent GetDeactivatedGroupPage() => 
            _documentTypeAliasProvider
                .GetGroupDeactivatedPage()
                .Map(GetXPathAtGroupRoom)
                .Map(GetContent);

        private IEnumerable<string> GetXPathAtGroupRoom(string pageAlias) => 
            OverviewXPath
                .Append(_documentTypeAliasProvider.GetGroupRoomPage())
                .Append(pageAlias);
    }
}