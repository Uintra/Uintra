using System.Collections.Generic;

namespace uCommunity.Navigation.DefaultImplementation
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
