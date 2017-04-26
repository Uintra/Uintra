using System.Collections.Generic;

namespace uCommunity.Navigation.DefaultImplementation
{
    public class SystemLinksViewModel
    {
        public List<SystemLinkItemViewModel> SystemLinks { get; set; }

        public SystemLinksViewModel()
        {
            SystemLinks = new List<SystemLinkItemViewModel>();
        }
    }
}
