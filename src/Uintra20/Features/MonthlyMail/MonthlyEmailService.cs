using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Events;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.News;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Entities;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Social;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.ApplicationSettings;
using Umbraco.Core.Logging;

namespace Uintra20.Features.MonthlyMail
{
    public class MonthlyEmailService: MonthlyEmailServiceBase
    {
        private readonly ISocialService<Social.Entities.Social> _bulletinsService;
        private readonly IEventsService<Event> _eventsService;
        private readonly INewsService<News.Entities.News> _newsService;
        private readonly IUserTagRelationService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;


        public MonthlyEmailService(IMailService mailService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ILogger logger,
            ISocialService<Social.Entities.Social> bulletinsService,
            IEventsService<Event> eventsService,
            INewsService<News.Entities.News> newsService,
            IUserTagRelationService userTagService,
            IActivityLinkService activityLinkService,
            INotificationSettingsService notificationSettingsService, 
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


        protected override IEnumerable<(IIntranetActivity activity, UintraLinkModel detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId)
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

        protected override MailBase GetMonthlyMailModel(IIntranetMember receiver, MonthlyMailDataModel dataModel, EmailNotifierTemplate template) =>
            _notificationModelMapper.Map(dataModel, template, receiver);

        protected virtual IEnumerable<IIntranetActivity> GetAllActivities()
        {
            var allBulletins = _bulletinsService.GetAll().Cast<IIntranetActivity>();
            var allNews = _newsService.GetAll().Cast<IIntranetActivity>();
			var allEvents = _eventsService.GetAll().Cast<IIntranetActivity>();

			return allBulletins.Concat(allNews).Concat(allEvents);
        }
    }
}