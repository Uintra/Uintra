using System.Collections.Generic;

namespace uIntra.Groups
{
    public class GroupMemberOverviewViewModel
    {
        public IEnumerable<GroupMemberViewModel> Members { get; set; }
        public bool CanExcludeFromGroup { get; set; }
    }
}
