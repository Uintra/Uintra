using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.Activity.Models;

namespace Uintra20.Features.News.Models
{
    public class UintraNewsDetailsPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public IntranetActivityDetailsViewModel Details { get; set; }
    }
}