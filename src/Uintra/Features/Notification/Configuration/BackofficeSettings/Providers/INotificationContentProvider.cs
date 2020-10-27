using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationContentProvider
    {
        IPublishedContent GetNotificationListPage();
    }
}
