using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Groups.ContentServices
{
    public class GroupContentProvider : FeedContentProviderBase, IGroupContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        protected override IEnumerable<string> OverviewAliasPath { get; }

        public GroupContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper)
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            OverviewAliasPath = new[] { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage() };
        }

        public override IEnumerable<IPublishedContent> GetRelatedPages() =>
            GetGroupRoomPage().Children;

        public IPublishedContent GetGroupRoomPage() =>
            _documentTypeAliasProvider.GetGroupRoomPage()
                .Pipe(OverviewAliasPath.Append)
                .Pipe(GetContent);

        public IPublishedContent GetCreateGroupPage() =>
            _documentTypeAliasProvider.GetGroupCreatePage()
                .Pipe(OverviewAliasPath.Append)
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
            OverviewAliasPath
                .Append(_documentTypeAliasProvider.GetGroupRoomPage())
                .Append(pageAlias);
    }
}