using System.Collections.Generic;

namespace Uintra.Core.Permissions.Models
{
    public class GroupPermissionsViewModel
    {
        public MemberGroupViewModel MemberGroup { get; set; }
        public bool IsSuperUser { get; set; }
        public IEnumerable<PermissionViewModel> Permissions { get; set; }
    }
}
