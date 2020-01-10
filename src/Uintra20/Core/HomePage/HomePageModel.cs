using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.PanelSettings;
using UBaseline.Shared.Property;

namespace Uintra20.Core.HomePage
{
    public class HomePageModel : NodeModel, IPanelSettingsComposition
    {
        public PanelSettingsCompositionModel PanelSettings { get; set; }
        public PropertyModel<PanelContainerModel> Panels {get;set;}
    }
}