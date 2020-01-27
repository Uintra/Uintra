using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;

namespace Uintra20.Features.News.Models
{
    public class UintraNewsEditPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public NewsViewModel Details { get; set; }
    }
}