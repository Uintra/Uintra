using System.Collections.Generic;
using uCommunity.Navigation.App_Plugins;

namespace uCommunity.Navigation
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
