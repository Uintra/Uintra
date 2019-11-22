using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelSettings;

namespace Uintra20.Features.CentralFeed
{
	public class CentralFeedPanelViewModel : FeedOverviewModel, INodeViewModel
	{
		public PanelSettingsCompositionViewModel PanelSettings { get; set; }
		public Enum Type { get; set; }
		public FeedTabSettings TabSettings { get; set; }
		public string ContentTypeAlias { get; set; }
		public string Name { get; set; }
		public bool AddToSitemap { get; set; }
		public string Url { get; set; }
	}
}