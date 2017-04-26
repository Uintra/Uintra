using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public class SystemLinksModel
    {
        public List<SystemLinkItemModel> SystemLinks { get; set; }

        public SystemLinksModel()
        {
            SystemLinks = new List<SystemLinkItemModel>();
        }
    }
}
