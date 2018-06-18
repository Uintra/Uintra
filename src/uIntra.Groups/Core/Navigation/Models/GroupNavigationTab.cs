using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public class GroupNavigationTab
    {
        public IPublishedContent Content { get; set; }
        public IIntranetType Type { get; set; }
        public string CreateUrl { get; set; }
        public bool IsActive { get; set; }
    }
}