using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public class NotificationContentProvider : ContentProviderBase, INotificationContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public NotificationContentProvider(
            UmbracoHelper umbracoHelper, 
            IDocumentTypeAliasProvider documentTypeAliasProvider)
            : base(umbracoHelper)
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