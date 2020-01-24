using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class LinksPicker
    {
        public string Caption { get; set; }
        public dynamic Link { get; set; }
        public string Target { get; set; }
    }
}