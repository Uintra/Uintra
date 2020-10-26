using UBaseline.Shared.Node;

namespace Uintra20.Features.Groups.ContentServices
{
    public interface IGroupContentProvider
    {
        NodeModel GetGroupsOverviewPage();
        NodeModel GetGroupCreatePage();
        NodeModel GetMyGroupsPage();
        NodeModel GetGroupRoomPage();
        NodeModel GetGroupEditPage();
        NodeModel GetGroupDocumentsPage();
        NodeModel GetGroupMembersPage();
    }
}
