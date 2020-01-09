using UBaseline.Shared.LocalScriptsComposition;
using UBaseline.Shared.MetaData;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra20.Features.News.Models
{
    public class NewsDetailsPageModel : NodeModel
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
        public MetaDataCompositionModel MetaData { get; set; }
        public LocalScriptsCompositionModel LocalScripts { get; set; }
    }
}