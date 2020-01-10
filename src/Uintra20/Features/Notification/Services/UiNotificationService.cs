using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using UBaseline.Core.Extensions;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;
using Uintra20.Features.Notification.ViewModel;

namespace Uintra20.Features.Notification.Services
{
    public class UiNotificationService : IUiNotificationService
    {
        private readonly ISqlRepository<Sql.Notification> _notificationRepository;
        public UiNotificationService(
            ISqlRepository<Sql.Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        #region async

        public async Task<IEnumerable<Sql.Notification>> GetManyAsync(Guid receiverId)
        {
            var notifications = await GetNotificationsAsync(receiverId);

            var orderedNotifications = notifications
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date);

            return orderedNotifications;
        }

        public async Task NotifyAsync(IEnumerable<Sql.Notification> notifications)
        {
            var notificationsList = notifications.AsList();
            foreach (var n in notificationsList)
            {
                n.IsNotified = true;
            }

            await _notificationRepository.UpdateAsync(notificationsList);
        }

        public async Task NotifyAsync(IEnumerable<UiNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Sql.Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    NotifierType = NotifierTypeEnum.UiNotifier.ToInt(),
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual, el.DesktopMessage, el.DesktopTitle, el.IsDesktopNotificationEnabled }.ToJson(),
                    ReceiverId = el.ReceiverId
                }).ToList();

            await _notificationRepository.AddAsync(notifications);
            SendNewUiNotificationsArrived(notifications);
        }

        public async Task<int> GetNotNotifiedCountAsync(Guid receiverId)
        {
            return (await GetNotificationsAsync(receiverId)).Count(el => !el.IsNotified);
        }

        public async Task<IEnumerable<Sql.Notification>> GetNotNotifiedAsync(Guid receiverId)
        {
            return (await GetNotificationsAsync(receiverId)).Where(el => !el.IsNotified);
        }

        public async Task<IEnumerable<Sql.Notification>> GetNotNotifiedNotificationsAsync(Guid receiverId)
        {
            var notifications = await GetNotificationsAsync(receiverId, true);
            return notifications;
        }

        public async Task ViewNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.GetAsync(id);
            notification.IsViewed = true;
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task<bool> SetNotificationAsNotifiedAsync(Guid id)
        {
            var notification = await _notificationRepository.GetAsync(id);
            if (notification.IsNotified)
            {
                return false;
            }
            notification.IsNotified = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        private async Task<IEnumerable<Sql.Notification>> GetNotificationsAsync(Guid receiverId, bool excludeAlreadyNotified = false)
        {
            var uiNotifierTypeId = NotifierTypeEnum.UiNotifier.ToInt();

            Expression<Func<Sql.Notification, bool>> basePredicate = notification =>
                notification.ReceiverId == receiverId &&
                notification.NotifierType == uiNotifierTypeId;

            var predicate = excludeAlreadyNotified
                ? basePredicate.AndAlso(notification => !notification.IsNotified)
                : basePredicate;

            return await _notificationRepository.FindAllAsync(predicate);
        }

        #endregion

        public IEnumerable<Sql.Notification> GetMany(Guid receiverId)
        {
            var notifications = GetNotifications(receiverId);

            var orderedNotifications = notifications
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date);

            return orderedNotifications;
        }

        public void Notify(IEnumerable<UiNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Sql.Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    NotifierType = NotifierTypeEnum.UiNotifier.ToInt(),
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual, el.DesktopMessage, el.DesktopTitle, el.IsDesktopNotificationEnabled }.ToJson(),
                    ReceiverId = el.ReceiverId
                }).ToList();

            _notificationRepository.Add(notifications);
            SendNewUiNotificationsArrived(notifications);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return GetNotifications(receiverId).Count(el => !el.IsNotified);
        }

        public IEnumerable<Sql.Notification> GetNotNotified(Guid receiverId)
        {
            return GetNotifications(receiverId).Where(el => !el.IsNotified);
        }

        public IEnumerable<Sql.Notification> GetNotNotifiedNotifications(Guid receiverId)
        {
            var notifications = GetNotifications(receiverId, true);
            return notifications;
        }

        public void ViewNotification(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            notification.IsViewed = true;
            _notificationRepository.Update(notification);
        }

        public void Notify(IEnumerable<Sql.Notification> notifications)
        {
            var notificationsList = notifications.AsList();
            foreach (var n in notificationsList)
            {
                n.IsNotified = true;
            }

            _notificationRepository.Update(notificationsList);
        }

        public bool SetNotificationAsNotified(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            if (notification.IsNotified)
            {
                return false;
            }
            notification.IsNotified = true;
            _notificationRepository.Update(notification);
            return true;
        }

        private IList<Sql.Notification> GetNotifications(Guid receiverId, bool excludeAlreadyNotified = false)
        {
            var uiNotifierTypeId = NotifierTypeEnum.UiNotifier.ToInt();

            Expression<Func<Sql.Notification, bool>> basePredicate = notification =>
                notification.ReceiverId == receiverId &&
                notification.NotifierType == uiNotifierTypeId;


            var predicate = excludeAlreadyNotified
                ? basePredicate.AndAlso(notification => !notification.IsNotified)
                : basePredicate;

            return _notificationRepository.FindAll(predicate);
        }

        private void SendNewUiNotificationsArrived(IEnumerable<Sql.Notification> notifications)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();

            notifications
                .GroupBy(m => m.ReceiverId)
                .ToList()
                .ForEach(r =>
                {
                    var notificationViewModels = GetNotNotified(r.Key).Map<IEnumerable<NotificationViewModel>>();
                    hubContext.Clients.User(r.Key.ToString()).updateNotifications(notificationViewModels);
                });
        }

    }
}