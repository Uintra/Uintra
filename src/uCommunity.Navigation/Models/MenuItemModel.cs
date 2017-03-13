using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Navigation
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public bool IsHomePage { get; set; }

        public IEnumerable<MenuItemModel> Children { get; set; }

        public MenuItemModel()
        {
            Children = Enumerable.Empty<MenuItemModel>();
        }
    }
}
