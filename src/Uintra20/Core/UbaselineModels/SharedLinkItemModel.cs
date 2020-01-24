using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class SharedLinkItemModel : NodeModel
    {
        public PropertyModel<string> LinksGroupTitle { get; set; }
        public PropertyModel<LinksPicker[]> Links { get; set; }
        public PropertyModel<int> Sort { get; set; }
    }
}