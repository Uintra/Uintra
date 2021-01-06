using System.Collections.Generic;
using System.Linq;

namespace Uintra.Features.Navigation.Models
{
    public class SubNavigationMenuItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public bool CurrentItem { get; set; }
        public bool ShowInMenu { get; set; }
        public IEnumerable<SubNavigationMenuItemModel> SubItems { get; set; }=Enumerable.Empty<SubNavigationMenuItemModel>();
    }
}