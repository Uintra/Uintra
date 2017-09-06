using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupNavigationTabViewModel
    {
        public IIntranetType Type { get; set; }        
        public string Title { get; set; }
        public string Url { get; set; }
        public string CreateUrl { get; set; }
        public bool IsActive { get; set; }
        public bool AlignRight { get; set; }
    }
}