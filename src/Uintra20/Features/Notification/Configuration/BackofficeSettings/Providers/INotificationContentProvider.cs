using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationContentProvider
    {
        IPublishedContent GetNotificationListPage();
    }
}
