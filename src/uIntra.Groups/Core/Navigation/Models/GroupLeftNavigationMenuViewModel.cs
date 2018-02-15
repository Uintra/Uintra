using System.Collections.Generic;
using System.Linq;

namespace Uintra.Groups
{
    public class GroupLeftNavigationMenuViewModel
    {
        public string GroupOverviewPageUrl { get; set; }
        public IEnumerable<GroupLeftNavigationItemViewModel> Items { get; set; } = Enumerable.Empty<GroupLeftNavigationItemViewModel>();
        public bool IsActive { get; set; }
    }
}
