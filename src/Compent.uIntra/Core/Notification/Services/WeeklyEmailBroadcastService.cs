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
using Uintra.Notification.Jobs;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.Notification
{
    public class WeeklyEmailBroadcastService : EmailBroadcastServiceBase<WeeklyMailBroadcast> 
    {
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;
        private readonly IApplicationSettings _applicationSettings;

        public WeeklyEmailBroadcastService(
            IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IExceptionLogger logger,
            NotificationSettingsService notificationSettingsService,
            IActivityLinkService activityLinkService,
            INewsService<NewsBase> newsService,
            IEventsService<EventBase> eventsService,
            IBulletinsService<BulletinBase> bulletinsService,
            IUserTagRelationService userTagService, 
            IApplicationSettings applicationSettings, 
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper)
            : base(mailService, intranetMemberService, logger, notificationSettingsService, activityLinkService, newsService, eventsService, bulletinsService, userTagService)
        {
            _applicationSettings = applicationSettings;
            _notificationModelMapper = notificationModelMapper;
        }

        public override void IsBroadcastable()
        {
            if (IsMonthlySendingDay()) Broadcast();
        }

        public override MailBase GetMailModel(IIntranetMember receiver, BroadcastMailModel model, EmailNotifierTemplate template)
        {
            return _notificationModelMapper.Map(model, template, receiver);
        }

        public virtual bool IsMonthlySendingDay()
        {
            var currentDate = DateTime.UtcNow.Day;

            return currentDate != _applicationSettings.MonthlyEmailJobDay;
        }
    }
}