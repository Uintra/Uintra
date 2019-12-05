using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Bulletins.Models;

namespace Uintra20.Core.Bulletin.Converters.Models
{
    public class BulletinDetailsPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public BulletinExtendedViewModel Details { get; set; }
    }
}