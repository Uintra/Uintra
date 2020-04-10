using System;
using System.Linq;
using System.Threading.Tasks;
using UBaseline.Core.Extensions;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Notification.Services
{
    public class MailNotifierService : INotifierService
    {
        private readonly IMailService _mailService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>
            _notificationModelMapper;

        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly ISqlRepository<Sql.Notification> _notificationRepository;

        public MailNotifierService(
            IMailService mailService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            INotificationSettingsService notificationSettingsService,
            ISqlRepository<Sql.Notification> notificationRepository)
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

                _notificationRepository.Add(new Sql.Notification()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = true,
                    IsViewed = false,
                    Type = data.NotificationType.ToInt(),
                    NotifierType = NotifierTypeEnum.EmailNotifier.ToInt(),
                    Value = new {message}.ToJson(),
                    ReceiverId = receiverId
                });
            }
        }

        public async Task NotifyAsync(NotifierData data)
        {
            var identity = GetSettingsIdentity(data);

            var settings = _notificationSettingsService.Get<EmailNotifierTemplate>(identity);
            if (settings != null && settings.IsEnabled)
            {
                var receivers = (await _intranetMemberService.GetManyAsync(data.ReceiverIds)).ToList();
                foreach (var receiverId in data.ReceiverIds)
                {
                    var user = receivers.Find(receiver => receiver.Id == receiverId);

                    var message = _notificationModelMapper.Map(data.Value, settings.Template, user);

                    await _mailService.SendAsync(message);

                    await _notificationRepository.AddAsync(new Sql.Notification
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.UtcNow,
                        IsNotified = true,
                        IsViewed = false,
                        Type = data.NotificationType.ToInt(),
                        NotifierType = NotifierTypeEnum.EmailNotifier.ToInt(),
                        Value = new {message}.ToJson(),
                        ReceiverId = receiverId
                    });
                }
            }
        }

        private ActivityEventNotifierIdentity GetSettingsIdentity(NotifierData data)
        {
            Enum activityType;

            switch (data.NotificationType.ToInt())
            {
                case (int) NotificationTypeEnum.CommentLikeAdded:
                case (int) NotificationTypeEnum.MonthlyMail:
                case (int) IntranetActivityTypeEnum.ContentPage:
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