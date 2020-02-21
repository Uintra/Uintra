using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Groups.Models
{
    public class GroupLinksModel
    {
        public UintraLinkModel GroupRoomPage { get; set; }
        public UintraLinkModel GroupDocumentsPage { get; set; }
        public UintraLinkModel GroupMembersPage { get; set; }
        public UintraLinkModel GroupEditPage { get; set; }
    }
}