using System.Collections.Generic;

namespace uIntra.Navigation.DefaultImplementation
{
    public class SubNavigationMenuViewModel
    {
        public string Title { get; set; }

        public List<MenuItemViewModel> Items { get; set; }

        public MenuItemViewModel Parent { get; set; }

        public SubNavigationMenuViewModel()
        {
            Items = new List<MenuItemViewModel>();
        }
    }
}
