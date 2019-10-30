using Compent.Extensions;
using System;
using System.Linq;
using Uintra.Bulletins;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Exceptions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.News;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.Notification
{
    public class MonthlyEmailBroadcastService
        : EmailBroadcastServiceBase<MonthlyMailBroadcast>
    {
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper;
        private readonly IApplicationSettings applicationSettings;
        private readonly IIntranetMemberService<IIntranetMember> intranetMemberService;
        private readonly NotificationSettingsService notificationSettingsService;
        private readonly IMailService mailService;
        private readonly IExceptionLogger logger;

        public MonthlyEmailBroadcastService(
            IActivityLinkService activityLinkService,
            INewsService<NewsBase> newsService,
            IEventsService<EventBase> eventsService,
            IBulletinsService<BulletinBase> bulletinsService,
            IUserTagRelationService userTagService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            IApplicationSettings applicationSettings,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            NotificationSettingsService notificationSettingsService,
            IMailService mailService, IExceptionLogger logger)
            : base(activityLinkService, newsService, eventsService, bulletinsService, userTagService)
        {
            this.notificationModelMapper = notificationModelMapper;
            this.applicationSettings = applicationSettings;
            this.intranetMemberService = intranetMemberService;
            this.notificationSettingsService = notificationSettingsService;
            this.mailService = mailService;
            this.logger = logger;
        }

        public override void IsBroadcastable()
        {
            var isMonthlySendingDay = IsMonthlySendingDay();

            if (isMonthlySendingDay) Broadcast();
        }

        public override void Broadcast()
        {
            var allUsers = intranetMemberService.GetAll();

            var monthlyMails = allUsers
                .Select(user => user.Id.Pipe(GetUserActivitiesFilteredByUserTags).Pipe(userActivities => TryGetBroadcastMail(userActivities, user)))
                .ToList();

            var identity = new ActivityEventIdentity(
                    CommunicationTypeEnum.CommunicationSettings,
                    NotificationTypeEnum.MonthlyMail)
                .AddNotifierIdentity(NotifierTypeEnum.EmailNotifier);

            var settings = notificationSettingsService.Get<EmailNotifierTemplate>(identity);

            if (!settings.IsEnabled) return;

            foreach (var monthlyMail in monthlyMails)
            {
                monthlyMail.Do(some: mail =>
                {
                    var mailModel = GetMailModel(mail.user, mail.monthlyMail, settings.Template);
                    try
                    {
                        mailService.SendMailByTypeAndDay(mailModel, mail.user.Email,DateTime.UtcNow, NotificationTypeEnum.MonthlyMail);
                    }
                    catch (Exception ex)
                    {
                        logger.Log(ex);
                    }
                });
            }
        }

        public override MailBase GetMailModel(
            IIntranetMember receiver,
            BroadcastMailModel model,
            EmailNotifierTemplate template)
        {
            var mappedNotification = notificationModelMapper.Map(model, template, receiver);

            return mappedNotification;
        }

        public virtual bool IsMonthlySendingDay()
        {
            var currentDate = DateTime.UtcNow;

            return currentDate.Day != applicationSettings.MonthlyEmailJobDay;
        }
    }
}