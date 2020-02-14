using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Shared.Node;
using Uintra20.Features.Groups.Models;

namespace Uintra20.Features.Groups.ContentServices
{
    public class GroupContentProvider : IGroupContentProvider
    {
        private readonly INodeModelService _nodeModelService;

        public GroupContentProvider(INodeModelService nodeModelService)
        {
            _nodeModelService = nodeModelService;
        }
        
        public NodeModel GetGroupsOverviewPage()
        {
            return _nodeModelService.AsEnumerable().OfType<UintraGroupsPageModel>().First();
        }

        public NodeModel GetGroupCreatePage()
        {
            return _nodeModelService.AsEnumerable().OfType<UintraGroupsCreatePageModel>().First();
        }

        public NodeModel GetMyGroupsPage()
        {
            return _nodeModelService.AsEnumerable().OfType<UintraMyGroupsPageModel>().First();
        }

        public NodeModel GetGroupRoomPage() => 
            _nodeModelService.AsEnumerable().OfType<UintraGroupsRoomPageModel>().First();

        public NodeModel GetGroupEditPage() =>
            _nodeModelService.AsEnumerable().OfType<UintraGroupsEditPageModel>().First();

        public NodeModel GetGroupDocumentsPage() =>
            _nodeModelService.AsEnumerable().OfType<UintraGroupsDocumentsPageModel>().First();

        public NodeModel GetGroupMembersPage() => 
            _nodeModelService.AsEnumerable().OfType<UintraGroupsMembersPageModel>().First();

         
    }
}