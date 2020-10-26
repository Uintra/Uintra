using System;

namespace Uintra.Features.Groups.Models
{
    public class GroupToggleAdminRightsModel
    {
        public Guid MemberId { get; set; }
        public Guid GroupId { get; set; }
    }
}