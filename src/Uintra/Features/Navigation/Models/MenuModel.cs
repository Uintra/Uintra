using System.Collections.Generic;

namespace Uintra.Features.Navigation.Models
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