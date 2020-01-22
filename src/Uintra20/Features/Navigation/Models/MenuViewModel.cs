using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Navigation.Models
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = Enumerable.Empty<MenuItemViewModel>();
    }
}