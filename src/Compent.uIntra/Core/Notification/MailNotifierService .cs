using System.Linq;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Services;
using uIntra.Notification.DefaultImplementation;

namespace Compent.uIntra.Core.Notification
{
    public class MailNotifierService : INotifierService
    {
        private readonly IMailService _mailService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly NotifierTypeProvider _notifierTypeProvider;

        public MailNotifierService(
            IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            NotifierTypeProvider notifierTypeProvider,
            NotificationSettingsService notificationSettingsService)
        {
            _mailService = mailService;
            _intranetUserService = intranetUserService;
            _notificationModelMapper = notificationModelMapper;
            _notifierTypeProvider = notifierTypeProvider;
            _notificationSettingsService = notificationSettingsService;
        }

        public NotifierTypeEnum Type => NotifierTypeEnum.EmailNotifier;

        public void Notify(NotifierData data)
        {
            var identity = new ActivityEventIdentity(data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(_notifierTypeProvider.Get((int) Type));
            var settings = _notificationSettingsService.Get<EmailNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds).ToList();
            foreach (var receiverId in data.ReceiverIds)
            {
                var user = receivers.Find(receiver => receiver.Id == receiverId);

                var message = _notificationModelMapper.Map(data.Value, settings.Template, user);
                _mailService.Send(message);
            }
        }
    }
}