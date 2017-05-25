using System.Collections.Generic;

namespace uIntra.Navigation
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
