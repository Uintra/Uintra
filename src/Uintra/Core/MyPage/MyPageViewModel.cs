using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Core.MyPage
{
    public class MyPageViewModel : NodeViewModel
    {
        public PropertyViewModel<bool> MyCustomToggle { get; set; }
        public PropertyViewModel<string> MyCustomTextArea { get; set; }
        public PropertyViewModel<string> Title { get; set; }
        public PropertyViewModel<bool> AllowAccess { get; set; }
    }
}