using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using Umbraco.Web;

namespace uIntra.Notification.Installer.Migrations
{
    public class OldUiNotificationMigration
    {
        private readonly ISqlRepository<Notification> _notificationRepository;

        public OldUiNotificationMigration()
        {
            _notificationRepository = GetService<ISqlRepository<Notification>>();
        }

        private T GetService<T>() => DependencyResolver.Current.GetService<T>();
        

        public void Execute()
        {
            var allNotifications = _notificationRepository
                .GetAll()
                .Select(n => (item: n, data: n.Value.Deserialize<OldNotifierData>()));

            var oldNotifications = allNotifications
                .Where(n => IsOldNotifierData(n.data))
                .ToList();

            var updatedNotifications = oldNotifications
                .Select(UpdateNotificationValue);

            _notificationRepository.Update(updatedNotifications);
        }

        private Notification UpdateNotificationValue((Notification item, OldNotifierData data) notification)
        {
            var item = notification.item;
            item.Value = MapToNewNotificationValue(notification).ToJson();
            return item;
        }

        private NotificationValue MapToNewNotificationValue((Notification item, OldNotifierData data) notification)
        {
            return new NotificationValue
            {
                Message = notification.data.Title,
                Url = notification.data.Url
            };
        }

        private bool IsOldNotifierData(OldNotifierData data)
        {
            return data.Title.IsNotNullOrEmpty();
        }

        private class OldNotifierData
        {
            public IntranetType ActivityType { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public Guid NotifierId { get; set; }
            public DateTime CreateDate { get; set; }    
        }
    }
}
