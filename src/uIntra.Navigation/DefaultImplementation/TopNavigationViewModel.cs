using Uintra.Core.User;

namespace Uintra.Navigation
{
    public class TopNavigationViewModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public string CentralUserListUrl { get; set; }
    }
}
