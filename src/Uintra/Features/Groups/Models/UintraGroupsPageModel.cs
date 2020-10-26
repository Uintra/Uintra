﻿using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.Property;

namespace Uintra.Features.Groups.Models
{
    public class UintraGroupsPageModel : NodeModel, IPanelsComposition, IGroupNavigationComposition, IPageSettingsComposition
    {
        public PropertyModel<PanelContainerModel> Panels { get; set; }
        public GroupNavigationCompositionModel GroupNavigation { get; set; }
        public PageSettingsCompositionModel PageSettings { get; set; }
    }
}