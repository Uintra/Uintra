using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Navigation.App_Plugins
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; }

        public MenuViewModel()
        {
            MenuItems = Enumerable.Empty<MenuItemViewModel>();
        }
    }
}
