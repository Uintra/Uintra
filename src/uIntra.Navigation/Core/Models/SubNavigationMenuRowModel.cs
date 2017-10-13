using System.Collections.Generic;

namespace uIntra.Navigation
{
    public class SubNavigationMenuRowModel
    {
        public IList<SubNavigationMenuItemModel> Items { get; set; } = new List<SubNavigationMenuItemModel>();
    }
}
