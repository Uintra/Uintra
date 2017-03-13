using System.Collections.Generic;

namespace uCommunity.Navigation.App_Plugins
{
    public class MenuViewModel
    {
        public List<MenuItemViewModel> MenuItems { get; set; }

        public MenuViewModel()
        {
            MenuItems = new List<MenuItemViewModel>();
        }
    }
}
