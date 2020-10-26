using UBaseline.Shared.ImagePicker;
using UBaseline.Shared.Link;
using UBaseline.Shared.Panel;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;

namespace Uintra.Features.UintraPanels.ImagePanel.Models
{
    public class ImagePanelViewModel : PanelViewModel
    {
        public PropertyViewModel<PictureViewModel> Image { get; set; }
        public PropertyViewModel<LinkViewModel> Link { get; set; }
        public PropertyViewModel<string> Description { get; set; }
        public PropertyViewModel<string> Title { get; set; }
        public PanelSettingsCompositionViewModel PanelSettings { get; set; }
    }
}