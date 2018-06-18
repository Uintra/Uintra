using System.Collections.Generic;
using System.Linq;

namespace Uintra.Navigation
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = Enumerable.Empty<MenuItemViewModel>();
    }
}
