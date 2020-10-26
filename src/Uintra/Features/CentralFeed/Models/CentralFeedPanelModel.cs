﻿using UBaseline.Shared.Node;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;

namespace Uintra.Features.CentralFeed.Models
{
	public class CentralFeedPanelModel: NodeModel, IPanelSettingsComposition
	{
		public PropertyModel<string> TabType { get; set; }
		public PanelSettingsCompositionModel PanelSettings { get; set; }
        public PropertyModel<int> ItemsPerRequest { get; set; }
    }
}