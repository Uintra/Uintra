using System.Collections.Generic;
using System.Linq;

namespace uIntra.Navigation.Core
{
    public class SubNavigationMenuModel
    {
        public string Title { get; set; }

        public IEnumerable<MenuItemModel> Items { get; set; }

        public MenuItemModel Parent { get; set; }

        public SubNavigationMenuModel()
        {
            Items = Enumerable.Empty<MenuItemModel>();
        }
    }
}
