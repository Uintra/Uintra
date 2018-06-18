using System.Collections.Generic;
using System.Linq;

namespace Uintra.Groups.Navigation.Models
{
    public class GroupNavigationViewModel
    {
        public IEnumerable<GroupNavigationActivityTabViewModel> ActivityTabs { get; set; } =
            Enumerable.Empty<GroupNavigationActivityTabViewModel>();

        public IEnumerable<GroupNavigationPageTabViewModel> PageTabs { get; set; } =
            Enumerable.Empty<GroupNavigationPageTabViewModel>();

        public string GroupTitle { get; set; }
        public string GroupUrl { get; set; }
    }
}