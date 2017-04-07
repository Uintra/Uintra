using uCommunity.Core.User;

namespace Compent.uCommunity.Core.Navigation
{
    public class TopMenuViewModel
    {
        public IIntranetUser CurrentUser { get; set; }
        public string NotificationsUrl { get; set; }
    }
}