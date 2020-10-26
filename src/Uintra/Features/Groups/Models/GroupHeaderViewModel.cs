using Uintra.Features.Links.Models;

namespace Uintra.Features.Groups.Models
{
    public class GroupHeaderViewModel
    {
        public string Title { get; set; }
        public UintraLinkModel RoomPageLink { get; set; }
        public GroupLinksModel GroupLinks { get; set; }
    }
}