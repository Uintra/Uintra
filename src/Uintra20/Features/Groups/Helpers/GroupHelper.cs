using Uintra20.Features.Groups.ContentServices;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Groups.Helpers
{
    public class GroupHelper : IGroupHelper
    {
        private readonly IGroupContentProvider _contentProvider;

        public GroupHelper(IGroupContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public bool IsGroupPage(IPublishedContent currentPage) =>
            _contentProvider.GetOverviewPage().IsAncestorOrSelf(currentPage);

        public bool IsGroupRoomPage(IPublishedContent currentPage) =>
            _contentProvider.GetGroupRoomPage().IsAncestorOrSelf(currentPage);
    }
}