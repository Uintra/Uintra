using System.Collections.Generic;

namespace Uintra20.Features.Navigation.Models
{
    public class SubNavigationMenuRowModel
    {
        public IList<SubNavigationMenuItemModel> Items { get; set; } = new List<SubNavigationMenuItemModel>();
    }
}