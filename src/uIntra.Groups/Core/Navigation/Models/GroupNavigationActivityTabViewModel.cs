using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupNavigationActivityTabViewModel : GroupNavigationTabViewModelBase
    {
        public IIntranetType Type { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}