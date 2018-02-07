using System;
using uIntra.Core.Links;

namespace uIntra.Groups
{
    public class GroupNavigationActivityTabViewModel : GroupNavigationTabViewModelBase
    {
        public Enum Type { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}