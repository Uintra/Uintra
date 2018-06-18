using Uintra.Core;
using Uintra.Notification;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.Uintra.Core.Notification
{
    public class NotificationContentProvider : ContentProviderBase, INotificationContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public NotificationContentProvider(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
            : base (umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetNotificationListPage()
        {
            var xPath = new[] { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetNotificationPage() };
            return GetContent(xPath);
        }
    }
}