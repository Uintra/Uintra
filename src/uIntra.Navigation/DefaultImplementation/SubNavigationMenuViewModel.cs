using System.Collections.Generic;

namespace uIntra.Navigation
{
    public class SubNavigationMenuViewModel
    {
        public string Title { get; set; }

        public bool IsTitleHidden { get; set; }

        public bool ShowBreadcrumbs { get; set; }

        public List<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();

        public MenuItemViewModel Parent { get; set; }
    }
}
