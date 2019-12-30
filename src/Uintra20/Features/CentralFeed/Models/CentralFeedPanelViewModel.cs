using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelSettings;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Features.CentralFeed.Models
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
        public NodeType NodeType { get; set; }
    }
}