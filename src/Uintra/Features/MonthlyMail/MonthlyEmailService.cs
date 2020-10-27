using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Events;
using Uintra.Features.Events.Entities;
using Uintra.Features.Links;
using Uintra.Features.Links.Models;
using Uintra.Features.News;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Entities;
using Uintra.Features.Notification.Entities.Base.Mails;
using Uintra.Features.Notification.Models.NotifierTemplates;
using Uintra.Features.Notification.Services;
using Uintra.Features.Social;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.ApplicationSettings;
using Umbraco.Core.Logging;

namespace Uintra.Features.MonthlyMail
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