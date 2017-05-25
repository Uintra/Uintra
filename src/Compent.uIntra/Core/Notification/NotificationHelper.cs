using uCommunity.Notification.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.Notification
{
    public class NotificationHelper : INotificationHelper
    {
        private readonly UmbracoHelper _umbracoHelper;

        public NotificationHelper(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IPublishedContent GetNotificationListPage()
        {
            return _umbracoHelper.TypedContent(1174);
        }
    }
}