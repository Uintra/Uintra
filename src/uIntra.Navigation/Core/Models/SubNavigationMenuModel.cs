using System.Collections.Generic;
using System.Linq;

namespace uIntra.Navigation
{
    public class SubNavigationMenuModel
    {
        public string Title { get; set; }

        public bool IsTitleHidden { get; set; }

        public IEnumerable<MenuItemModel> Items { get; set; } = Enumerable.Empty<MenuItemModel>();

        public MenuItemModel Parent { get; set; }
    }
}
