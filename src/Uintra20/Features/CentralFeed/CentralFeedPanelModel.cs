using UBaseline.Shared.Node;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;

namespace Uintra20.Features.CentralFeed
{
	public class CentralFeedPanelModel: NodeModel, IPanelSettingsComposition
	{
		public PropertyModel<string> TabType { get; set; }
		public PanelSettingsCompositionModel PanelSettings { get; set; }
	}
}