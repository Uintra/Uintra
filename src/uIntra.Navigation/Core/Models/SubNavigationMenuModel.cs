using System.Collections.Generic;
using System.Linq;

namespace Uintra.Navigation
{
    public class SubNavigationMenuModel
    {
        public string Title { get; set; }

        public bool IsTitleHidden { get; set; }

        public IEnumerable<SubNavigationMenuRowModel> Rows { get; set; } = Enumerable.Empty<SubNavigationMenuRowModel>();
        public bool ShowBreadcrumbs { get; set; }


        public MenuItemModel Parent { get; set; }
    }
}
