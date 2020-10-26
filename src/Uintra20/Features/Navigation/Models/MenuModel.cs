using System.Collections.Generic;

namespace Uintra20.Features.Navigation.Models
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