using System.Collections.Generic;
using System.Linq;

namespace uIntra.Navigation
{
    public class SubNavigationMenuRowModel
    {
        public IEnumerable<SubNavigationMenuItemModel> Items { get; set; } = Enumerable.Empty<SubNavigationMenuItemModel>();
    }
}
