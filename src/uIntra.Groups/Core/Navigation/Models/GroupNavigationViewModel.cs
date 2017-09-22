using System.Collections.Generic;
using System.Linq;

namespace uIntra.Groups
{
    public class GroupNavigationViewModel
    {
        public IEnumerable<GroupNavigationActivityTabViewModel> ActivityTabs { get; set; }
        public IEnumerable<GroupNavigationPageTabViewModel> PageTabs { get; set; }
        public string GroupTitle { get; set; }
        public string GroupUrl { get; set; }
    }
}