using UBaseline.Shared.LocalScriptsComposition;
using UBaseline.Shared.MetaData;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;

namespace Uintra20.Features.News.Converters.Models
{
    public class NewsDetailsPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public MetaDataCompositionViewModel MetaData { get; set; }
        public LocalScriptsCompositionViewModel LocalScripts { get; set; }
    }
}