using System.Collections.Generic;

namespace uIntra.Navigation.Core
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
