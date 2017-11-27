using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Exceptions;

namespace uIntra.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IEnumerable<INotifierService> _notifiers;

        public NotificationsService(
            IExceptionLogger exceptionLogger,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IEnumerable<INotifierService> notifiers)
        {
            _exceptionLogger = exceptionLogger;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _notifiers = notifiers;
        }

        public void ProcessNotification(NotifierData data)
        {
            var allReceiversIds = data.ReceiverIds.ToList();
            var allReceiversNotifiersSettings = _memberNotifiersSettingsService.GetForMembers(allReceiversIds);

            (IEnumerable<Guid> receiverIds, bool isNotEmpty) GetReceiverIdsForNotifier(NotifierTypeEnum notifierType)
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