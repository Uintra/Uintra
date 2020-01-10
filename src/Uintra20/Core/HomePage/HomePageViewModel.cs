using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.HomePage
{
    public class HomePageViewModel : INodeViewModel
    {
        public string ContentTypeAlias { get; set; }
        public string Name { get; set; }
        public bool AddToSitemap { get; set; }
        public string Url { get; set; }
        public NodeType NodeType { get; set; }
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
    }
}