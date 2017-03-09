using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Navigation
{
    public class MenuModel
    {
        public IEnumerable<MenuItemModel> MenuItems { get; set; }

        public MenuModel()
        {
            MenuItems = Enumerable.Empty<MenuItemModel>();
        }
    }
}
