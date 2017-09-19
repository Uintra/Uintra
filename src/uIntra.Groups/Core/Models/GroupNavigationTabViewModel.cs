using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupNavigationTabViewModel
    {
        public IIntranetType Type { get; set; }        
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool AlignRight { get; set; }
        public ActivityCreateLinks Links { get; set; }
    }
}