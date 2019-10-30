using System;
using Uintra.Bulletins;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Exceptions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.News;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.Notification
{
    public class MonthlyEmailBroadcastService
        : EmailBroadcastServiceBase<MonthlyMailBroadcast>
    {
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;
        private readonly IApplicationSettings _applicationSettings;

        public MonthlyEmailBroadcastService(
            IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IExceptionLogger logger,
            IBulletinsService<BulletinBase> bulletinsService,
            IEventsService<EventBase> eventsService,
            INewsService<NewsBase> newsService,
            IUserTagRelationService userTagService,
            IActivityLinkService activityLinkService,
            NotificationSettingsService notificationSettingsService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            IApplicationSettings applicationSettings)
            : base(mailService, intranetMemberService, logger, notificationSettingsService, activityLinkService, newsService, eventsService, bulletinsService, userTagService)
        {
            _notificationModelMapper = notificationModelMapper;
            _applicationSettings = applicationSettings;
        }

        public override void IsBroadcastable()
        {
            var isMonthlySendingDay = IsMonthlySendingDay();

            if (isMonthlySendingDay) Broadcast();
        }

        public override MailBase GetMailModel(
            IIntranetMember receiver,
            BroadcastMailModel model,
            EmailNotifierTemplate template)
        {
            var mappedNotification = _notificationModelMapper.Map(model, template, receiver);

            return mappedNotification;
        }

        public virtual bool IsMonthlySendingDay()
        {
            var currentDate = DateTime.UtcNow;

            return currentDate.Day != _applicationSettings.MonthlyEmailJobDay;
        }
    }
}