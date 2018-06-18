using System.Collections.Generic;
using System.Linq;
using Uintra.CentralFeed.Providers;
using Uintra.Core;
using Compent.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Groups
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
                .Pipe(OverviewXPath.Append)
                .Pipe(GetContent);

        public IPublishedContent GetCreateGroupPage() =>
            _documentTypeAliasProvider.GetGroupCreatePage()
                .Pipe(OverviewXPath.Append)
                .Pipe(GetContent);

        public IPublishedContent GetEditPage() =>
            _documentTypeAliasProvider.GetGroupEditPage()
                .Pipe(GetXPathAtGroupRoom)
                .Pipe(GetContent);

        public IPublishedContent GetMyGroupsOverviewPage() => 
            _documentTypeAliasProvider
                .GetGroupMyGroupsOverviewPage()
                .Pipe(GetXPathAtGroupRoom)
                .Pipe(GetContent);

        public IPublishedContent GetDeactivatedGroupPage() => 
            _documentTypeAliasProvider
                .GetGroupDeactivatedPage()
                .Pipe(GetXPathAtGroupRoom)
                .Pipe(GetContent);

        private IEnumerable<string> GetXPathAtGroupRoom(string pageAlias) => 
            OverviewXPath
                .Append(_documentTypeAliasProvider.GetGroupRoomPage())
                .Append(pageAlias);
    }
}