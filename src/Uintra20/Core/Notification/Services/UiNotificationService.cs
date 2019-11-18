﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using Uintra20.Core.Extensions;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Persistence;
using static LanguageExt.Prelude;

namespace Uintra20.Core.Notification
{
    public class UiNotificationService : IUiNotificationService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;

        public UiNotificationService(ISqlRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        #region async

        public async Task<(IEnumerable<Notification> notifications, int totalCount)> GetManyAsync(Guid receiverId, int count)
        {
            var notifications = await GetNotificationsAsync(receiverId);

            var orderedNotifications = notifications
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date);

            return (orderedNotifications, notifications.Count);
        }

        public async Task NotifyAsync(IEnumerable<Notification> notifications)
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
                .Select(el => new Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    NotifierType = NotifierTypeEnum.UiNotifier.ToInt(),
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual, el.DesktopMessage, el.DesktopTitle, el.IsDesktopNotificationEnabled }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            await _notificationRepository.AddAsync(notifications);
        }

        public async Task<int> GetNotNotifiedCountAsync(Guid receiverId)
        {
            return (await GetNotificationsAsync(receiverId)).Count(el => !el.IsNotified);
        }

        public async Task<IList<Notification>> GetNotNotifiedNotificationsAsync(Guid receiverId)
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

        private async Task<IList<Notification>> GetNotificationsAsync(Guid receiverId, bool excludeAlreadyNotified = false)
        {
            var uiNotifierTypeId = NotifierTypeEnum.UiNotifier.ToInt();

            var basePredicate = expr((Notification notification) =>
                notification.ReceiverId == receiverId &&
                notification.NotifierType == uiNotifierTypeId);

            var predicate = excludeAlreadyNotified
                ? basePredicate.AndAlso(notification => !notification.IsNotified)
                : basePredicate;

            return (await _notificationRepository.FindAllAsync(predicate)).ToList();
        }

        #endregion

        #region sync

        public (IEnumerable<Notification> notifications, int totalCount) GetMany(Guid receiverId, int count)
        {
            var notifications = GetNotifications(receiverId);

            var orderedNotifications = notifications
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date);

            return (orderedNotifications, notifications.Count);
        }

        public void Notify(IEnumerable<UiNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    NotifierType = NotifierTypeEnum.UiNotifier.ToInt(),
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual, el.DesktopMessage, el.DesktopTitle, el.IsDesktopNotificationEnabled }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            _notificationRepository.Add(notifications);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return GetNotifications(receiverId).Count(el => !el.IsNotified);
        }

        public IList<Notification> GetNotNotifiedNotifications(Guid receiverId)
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

        public void Notify(IEnumerable<Notification> notifications)
        {
            var notificationsList = notifications.AsList();
            foreach (var n in notificationsList)
            {
                n.IsNotified = true;
            }

            _notificationRepository.Update(notificationsList);
        }

        private IList<Notification> GetNotifications(Guid receiverId, bool excludeAlreadyNotified = false)
        {
            var uiNotifierTypeId = NotifierTypeEnum.UiNotifier.ToInt();

            var basePredicate = expr((Notification notification) =>
                notification.ReceiverId == receiverId &&
                notification.NotifierType == uiNotifierTypeId);

            var predicate = excludeAlreadyNotified
                ? basePredicate.AndAlso(notification => !notification.IsNotified)
                : basePredicate;

            return _notificationRepository.FindAll(predicate);
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

        #endregion

    }
}