using System.Collections.Generic;

namespace uIntra.Groups
{
    public class GroupsListModel
    {
        public IEnumerable<GroupViewModel> Groups { get; set; }

        public bool BlockScrolling { get; set; }
    }
}