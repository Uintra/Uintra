using Uintra.Features.Links.Models;

namespace Uintra.Features.Groups.Models
{
    public class GroupLinksModel
    {
        public UintraLinkModel GroupRoomPage { get; set; }
        public UintraLinkModel GroupDocumentsPage { get; set; }
        public UintraLinkModel GroupMembersPage { get; set; }
        public UintraLinkModel GroupEditPage { get; set; }
    }
}