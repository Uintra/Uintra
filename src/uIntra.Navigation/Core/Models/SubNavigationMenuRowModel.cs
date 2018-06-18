using System.Collections.Generic;

namespace Uintra.Navigation
{
    public class SubNavigationMenuRowModel
    {
        public IList<SubNavigationMenuItemModel> Items { get; set; } = new List<SubNavigationMenuItemModel>();
    }
}
