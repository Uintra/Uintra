using System.Collections.Generic;
using System.Linq;

namespace uIntra.Groups
{
    public class GroupNavigationViewModel
    {
        public IEnumerable<GroupNavigationTabViewModel> Tabs { get; set; }

        public IEnumerable<GroupNavigationCreateTabViewModel> CreateTabs { get; set; }

        public string GroupTitle { get; set; }

        public string GroupUrl { get; set; }

        public GroupNavigationViewModel()
        {
            Tabs = Enumerable.Empty<GroupNavigationTabViewModel>();
            CreateTabs = Enumerable.Empty<GroupNavigationCreateTabViewModel>();
        }
    }
}