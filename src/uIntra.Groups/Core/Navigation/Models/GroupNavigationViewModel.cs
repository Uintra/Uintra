using System.Collections.Generic;
using System.Linq;
using Uintra.Groups.Navigation.Models;

namespace Uintra.Groups
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