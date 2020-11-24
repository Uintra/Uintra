using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using UBaseline.Shared.Title;
using Uintra.Core.Authentication;
using Uintra.Core.Authentication.Models;

namespace Uintra.Core.MyPage
{
    public class MyPageModel : NodeModel, ITitleContainer, IAnonymousAccessComposition
    {
        public PropertyModel<bool> MyCustomToggle { get; set; }
        public PropertyModel<string> MyCustomTextArea { get; set; }
        public PropertyModel<string> Title { get; set; }
        public PropertyModel<bool> AllowAccess { get; set; }
    }
}