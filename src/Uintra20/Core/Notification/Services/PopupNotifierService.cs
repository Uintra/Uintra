using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Core.User;

namespace Uintra20.Core.Notification
{
    public class PopupNotifierService : INotifierService
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage> _notificationModelMapper;
        private readonly IPopupNotificationService _notificationsService;
        public Enum Type => NotifierTypeEnum.PopupNotifier;

        public PopupNotifierService(
            INotificationSettingsService notificationSettingsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage> notificationModelMapper,
            IPopupNotificationService notificationsService
        )
        {
            _notificationSettingsService = notificationSettingsService;
            _intranetMemberService = intranetMemberService;
            _notificationModelMapper = notificationModelMapper;
            _notificationsService = notificationsService;
        }

        public void Notify(NotifierData data)
        {
            var identity = new ActivityEventIdentity(CommunicationTypeEnum.Member, data.NotificationType).AddNotifierIdentity(Type);
            var settings = _notificationSettingsService.Get<PopupNotifierTemplate>(identity);

            if (settings == null || !settings.IsEnabled) return;
            var receivers = _intranetMemberService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }

        public async Task NotifyAsync(NotifierData data)
        {
            var identity = new ActivityEventIdentity(CommunicationTypeEnum.Member, data.NotificationType).AddNotifierIdentity(Type);
            var settings = await _notificationSettingsService.GetAsync<PopupNotifierTemplate>(identity);

            if (settings == null || !settings.IsEnabled) return;
            var receivers = await _intranetMemberService.GetManyAsync(data.ReceiverIds);

            var messages = await receivers.SelectAsync(async r => await _notificationModelMapper.MapAsync(data.Value, settings.Template, r));
            await _notificationsService.NotifyAsync(messages);
        }
    }
}