using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Exceptions;
using Uintra.Core.Persistence;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IEnumerable<INotifierService> _notifiers;

        public NotificationsService(
            ISqlRepository<Notification> notificationRepository,
            IExceptionLogger exceptionLogger,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IEnumerable<INotifierService> notifiers)
        {
            _notificationRepository = notificationRepository;
            _exceptionLogger = exceptionLogger;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _notifiers = notifiers;
        }

        public void ProcessNotification(NotifierData data)
        {
            var allReceiversIds = data.ReceiverIds.ToList();
            var allReceiversNotifiersSettings = _memberNotifiersSettingsService.GetForMembers(allReceiversIds);

            (IEnumerable<Guid> receiverIds, bool isNotEmpty) GetReceiverIdsForNotifier(Enum notifierType)
            {
                var ids = allReceiversIds
                    .Where(receiverId => allReceiversNotifiersSettings[receiverId].Contains(notifierType))
                    .ToList();
                return (ids, ids.Any());
            }

            foreach (var notifier in _notifiers)
            {
                var filterResult = GetReceiverIdsForNotifier(notifier.Type);
                if (filterResult.isNotEmpty) Notify(notifier, data);
            }
        }

        public IEnumerable<Notification> GetUserNotifications(Guid userId, int notificationTypeId)
        {
            return _notificationRepository.FindAll(n => n.ReceiverId == userId && n.Type == notificationTypeId);
        }

        protected void Notify(INotifierService service, NotifierData data)
        {
            try
            {
                service.Notify(data);
            }
            catch (Exception ex)
            {
                _exceptionLogger.Log(ex);
            }
        }
    }
}