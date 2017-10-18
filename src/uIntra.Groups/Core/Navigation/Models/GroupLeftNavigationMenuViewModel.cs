using System.Collections.Generic;
using System.Linq;

namespace uIntra.Groups
{
    public class GroupLeftNavigationMenuViewModel
    {
        public string GroupOverviewPageUrl { get; set; }
        public IEnumerable<GroupLeftNavigationItemViewModel> Items { get; set; } = Enumerable.Empty<GroupLeftNavigationItemViewModel>();
    }
}
