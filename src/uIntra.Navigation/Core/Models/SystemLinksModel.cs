using System.Collections.Generic;

namespace Uintra.Navigation
{
    public class SystemLinksModel
    {
        public string LinksGroupTitle { get; set; }
        public int SortOrder { get; set; }
        public List<SystemLinkItemModel> SystemLinks { get; set; }

        public SystemLinksModel()
        {
            SystemLinks = new List<SystemLinkItemModel>();
        }
    }
}
