using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Features.Groups;
using Uintra20.Features.Groups.Models;

namespace Uintra20.Features.News.Models
{
    public class UintraNewsCreatePageViewModel : NodeViewModel, IGroupHeader
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public NewsCreateDataViewModel Data { get; set; }
        public bool CanCreate { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}