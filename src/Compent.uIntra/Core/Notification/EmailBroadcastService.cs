using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Bulletins;
using Uintra.Core.Activity;
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
    public class EmailBroadcastService : EmailBroadcastServiceBase
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IEventsService<EventBase> _eventsService;
        private readonly INewsService<NewsBase> _newsService;
        private readonly IUserTagRelationService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;

        public EmailBroadcastService(
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
            : base(mailService, intranetMemberService, logger, notificationSettingsService, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            _eventsService = eventsService;
            _newsService = newsService;
            _userTagService = userTagService;
            _activityLinkService = activityLinkService;
            _notificationModelMapper = notificationModelMapper;
        }


        protected override IEnumerable<(IIntranetActivity activity, string detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId)
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

        protected override MailBase GetMonthlyMailModel(
            IIntranetMember receiver,
            MonthlyMailDataModel dataModel,
            EmailNotifierTemplate template)
        {
            return _notificationModelMapper.Map(dataModel, template, receiver);
        }

        protected virtual IEnumerable<IIntranetActivity> GetAllActivities()
        {
            var allBulletins = _bulletinsService.GetAll().Cast<IIntranetActivity>();

            var allNews = _newsService.GetAll().Cast<IIntranetActivity>();

            var allEvents = _eventsService.GetAll().Cast<IIntranetActivity>();

            return allBulletins.Concat(allNews).Concat(allEvents);
        }
    }
}