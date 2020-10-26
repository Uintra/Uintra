using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Navigation.Models.MyLinks;

namespace Uintra20.Features.Navigation.Models
{
    public class LeftNavigationViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = Enumerable.Empty<MenuItemViewModel>();
        public GroupLeftNavigationMenuViewModel GroupItems { get; set; }
        public IEnumerable<SharedLinkApiViewModel> SharedLinks { get; set; }
        public IEnumerable<MyLinkItemViewModel> MyLinks { get; set; }
    }
}