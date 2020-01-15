using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Bulletins.Models
{
    public class SocialDetailsPageModel : NodeModel
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
    }
}