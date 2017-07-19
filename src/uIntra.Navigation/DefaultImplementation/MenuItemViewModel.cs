using System.Collections.Generic;

namespace uIntra.Navigation
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsClickable { get; set; }

        public List<MenuItemViewModel> Children { get; set; }

        public MenuItemViewModel()
        {
            Children = new List<MenuItemViewModel>();
        }
    }
}
