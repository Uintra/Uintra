using UBaseline.Shared.ImagePicker;
using UBaseline.Shared.Link;
using UBaseline.Shared.Panel;
using UBaseline.Shared.Property;

namespace Uintra20.Features.UintraPanels.ImagePanel.Models
{
    public class ImagePanelModel : PanelModel
    {
        public PropertyModel<PictureModel> Image { get; set; }
        public PropertyModel<LinkModel> Link { get; set; }
        public PropertyModel<string> Description { get; set; }
        public PropertyModel<string> Title { get; set; }
    }
}