using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uintra.Bulletins;
using Uintra.Core.Activity;
using Uintra.Core.Exceptions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.News;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.Jobs;
using Uintra.Tagging.UserTags;

namespace Uintra.Notification
{
    public abstract class EmailBroadcastServiceBase<T> 
        : IEmailBroadcastService<T> where T : IMailBroadcast
    {
        private readonly IMailService _mailService;
        private readonly IExceptionLogger _logger;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IEventsService<EventBase> _eventsService;
        private readonly INewsService<NewsBase> _newsService;
        private readonly IUserTagRelationService _userTagService;
        private readonly IActivityLinkService _activityLinkService;

        protected EmailBroadcastServiceBase(
            IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IExceptionLogger logger,
            NotificationSettingsService notificationSettingsService, 
            IActivityLinkService activityLinkService, 
            INewsService<NewsBase> newsService, 
            IEventsService<EventBase> eventsService, 
            IBulletinsService<BulletinBase> bulletinsService, 
            IUserTagRelationService userTagService)
        {
            _mailService = mailService;
            _intranetMemberService = intranetMemberService;
            _logger = logger;
            _notificationSettingsService = notificationSettingsService;
            _activityLinkService = activityLinkService;
            _newsService = newsService;
            _eventsService = eventsService;
            _bulletinsService = bulletinsService;
            _userTagService = userTagService;
        }

        public abstract void IsBroadcastable();

        public abstract MailBase GetMailModel(IIntranetMember receiver, BroadcastMailModel model, EmailNotifierTemplate template);

        public IEnumerable<(IIntranetActivity activity, string detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId)
        {
            var allActivities = GetAllActivities()
                .Select(activity => (activity: activity, activityTagIds: _userTagService.GetForEntity(activity.Id)));

            var userTagIds = _userTagService
                .GetForEntity(userId)
                .ToList();

            var result = allActivities
                .Where(pair => userTagIds.Intersect(pair.activityTagIds).Any())
                .Select(pair => (pair.activity, detailsLink: _activityLinkService.GetLinks(pair.activity.Id).Details));

            return result;
        }

        public void Broadcast()
        {
            var allUsers = _intranetMemberService.GetAll();

            var monthlyMails = allUsers
                .Select(user => user.Id.Pipe(GetUserActivitiesFilteredByUserTags).Pipe(userActivities => TryGetBroadcastMail(userActivities, user)))
                .ToList();

            var identity = new ActivityEventIdentity(
                    CommunicationTypeEnum.CommunicationSettings,
                    NotificationTypeEnum.MonthlyMail)
                .AddNotifierIdentity(NotifierTypeEnum.EmailNotifier);

            var settings = _notificationSettingsService.Get<EmailNotifierTemplate>(identity);

            if (!settings.IsEnabled) return;

            foreach (var monthlyMail in monthlyMails)
            {
                monthlyMail.Do(some: mail =>
                {
                    var mailModel = GetMailModel(mail.user, mail.monthlyMail, settings.Template);
                    try
                    {
                        _mailService.SendMailByTypeAndDay(
                            mailModel,
                            mail.user.Email,
                            DateTime.UtcNow,
                            NotificationTypeEnum.MonthlyMail);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex);
                    }
                });
            }
        }

        public (IIntranetMember user, BroadcastMailModel monthlyMail)? TryGetBroadcastMail(
            IEnumerable<(IIntranetActivity activity, string detailsLink)> activities,
            IIntranetMember member)
        {
            var activityList = activities.AsList();

            if (activityList.Any())
            {
                var monthlyMail = new BroadcastMailModel
                {
                    ActivityList = GetActivityListString(activityList)
                };

                return (member, monthlyMail);
            }
            else
            {
                return default;
            }
        }

        public string GetActivityListString(IEnumerable<(IIntranetActivity activity, string link)> activities)
        {
            return activities
                .Aggregate(
                    new StringBuilder(),
                    (builder, activity) =>
                        builder.AppendLine($"<a href='{activity.link}'>{activity.activity.Title}</a></br>"))
                .ToString();
        }

        public virtual IEnumerable<IIntranetActivity> GetAllActivities()
        {
            var allBulletins = _bulletinsService.GetAll().Cast<IIntranetActivity>();

            var allNews = _newsService.GetAll().Cast<IIntranetActivity>();

            var allEvents = _eventsService.GetAll().Cast<IIntranetActivity>();

            return allBulletins.Concat(allNews).Concat(allEvents);
        }
    }
}
