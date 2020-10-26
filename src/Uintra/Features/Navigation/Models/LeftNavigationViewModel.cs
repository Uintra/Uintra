using System.Collections.Generic;
using System.Linq;
using Uintra.Features.Groups.Models;
using Uintra.Features.Navigation.Models.MyLinks;

namespace Uintra.Features.Navigation.Models
{
    public class LeftNavigationViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = Enumerable.Empty<MenuItemViewModel>();
        public GroupLeftNavigationMenuViewModel GroupItems { get; set; }
        public IEnumerable<SharedLinkApiViewModel> SharedLinks { get; set; }
        public IEnumerable<MyLinkItemViewModel> MyLinks { get; set; }
    }
}