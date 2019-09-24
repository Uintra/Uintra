using System;
using System.ComponentModel.DataAnnotations;

namespace Uintra.Groups
{
    public class GroupToggleAdminRightsModel
    {
        [Required(AllowEmptyStrings = false)]
        public Guid MemberId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public Guid GroupId { get; set; }
    }
}