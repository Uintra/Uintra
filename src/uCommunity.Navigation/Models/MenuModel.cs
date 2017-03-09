using System.Collections.Generic;

namespace uCommunity.Navigation
{
    public class MenuModel
    {
        public List<MenuItemModel> MenuItems { get; set; }

        public MenuModel()
        {
            MenuItems = new List<MenuItemModel>();
        }
    }
}
