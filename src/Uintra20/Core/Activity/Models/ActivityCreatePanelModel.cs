using UBaseline.Shared.Panel;
using UBaseline.Shared.Property;

namespace Uintra20.Core.Activity.Models
{
    public class ActivityCreatePanelModel : PanelModel
    {
        public PropertyModel<string> TabType { get; set; }
    }
}