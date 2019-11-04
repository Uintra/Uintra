using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Extensions;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Core.User;
using Uintra20.Persistence.Sql;

namespace Uintra20.Core.Notification
{
    public class MailNotifierService : INotifierService
    {
        private readonly IMailService _mailService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly ISqlRepository<Notification> _notificationRepository;

        public MailNotifierService(
            IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            NotificationSettingsService notificationSettingsService,
            ISqlRepository<Notification> notificationRepository)
        {
            _mailService = mailService;
            _intranetMemberService = intranetMemberService;
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _notificationRepository = notificationRepository;
        }

        public Enum Type => NotifierTypeEnum.EmailNotifier;

        public void Notify(NotifierData data)
        {
            var identity = GetSettingsIdentity(data);

            var settings = _notificationSettingsService.Get<EmailNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetMemberService.GetMany(data.ReceiverIds).ToList();
            foreach (var receiverId in data.ReceiverIds)
            {
                var user = receivers.Find(receiver => receiver.Id == receiverId);

                var message = _notificationModelMapper.Map(data.Value, settings.Template, user);
                _mailService.Send(message);

                _notificationRepository.Add(new Notification()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = true,
                    IsViewed = false,
                    Type = data.NotificationType.ToInt(),
                    NotifierType = NotifierTypeEnum.EmailNotifier.ToInt(),
                    Value = new { message }.ToJson(),
                    ReceiverId = receiverId
                });
            }
        }

        public async Task NotifyAsync(NotifierData data)
        {
            var identity = GetSettingsIdentity(data);

            var settings = await _notificationSettingsService.GetAsync<EmailNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetMemberService.GetMany(data.ReceiverIds).ToList();
            foreach (var receiverId in data.ReceiverIds)
            {
                var user = receivers.Find(receiver => receiver.Id == receiverId);

                var message = _notificationModelMapper.Map(data.Value, settings.Template, user);
                await _mailService.SendAsync(message);

                await _notificationRepository.AddAsync(new Notification()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = true,
                    IsViewed = false,
                    Type = data.NotificationType.ToInt(),
                    NotifierType = NotifierTypeEnum.EmailNotifier.ToInt(),
                    Value = new { message }.ToJson(),
                    ReceiverId = receiverId
                });
            }
        }

        private ActivityEventNotifierIdentity GetSettingsIdentity(NotifierData data)
        {
            Enum activityType;

            switch (data.NotificationType.ToInt())
            {
                case (int)NotificationTypeEnum.CommentLikeAdded:
                case (int)NotificationTypeEnum.MonthlyMail:
                case (int)IntranetActivityTypeEnum.ContentPage:
                case (int)IntranetActivityTypeEnum.PagePromotion:
                    activityType = CommunicationTypeEnum.CommunicationSettings;
                    break;
                default:
                    activityType = data.ActivityType;
                    break;
            }

            return new ActivityEventIdentity(activityType, data.NotificationType)
                .AddNotifierIdentity(Type);
        }
    }
}