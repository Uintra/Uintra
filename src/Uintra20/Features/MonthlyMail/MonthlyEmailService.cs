using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Links;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Entities;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Exceptions;

namespace Uintra20.Features.MonthlyMail
{
    public class MonthlyEmailService: MonthlyEmailServiceBase
    {
	    private readonly ISocialsService<SocialBase> _bulletinsService;
	    //todo uncomment when News and Events will be done
        //private readonly IEventsService<EventBase> _eventsService;
        //private readonly INewsService<NewsBase> _newsService;
        private readonly IUserTagRelationService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _notificationModelMapper;


        public MonthlyEmailService(IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IExceptionLogger logger,
            ISocialsService<SocialBase> bulletinsService,
            //IEventsService<EventBase> eventsService,
            //INewsService<NewsBase> newsService,
            IUserTagRelationService userTagService,
            IActivityLinkService activityLinkService,
            NotificationSettingsService notificationSettingsService, 
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> notificationModelMapper,
            IApplicationSettings applicationSettings) 
            : base(mailService, intranetMemberService, logger, notificationSettingsService, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            //_eventsService = eventsService;
            //_newsService = newsService;
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

        protected override MailBase GetMonthlyMailModel(IIntranetMember receiver, MonthlyMailDataModel dataModel, EmailNotifierTemplate template) =>
            _notificationModelMapper.Map(dataModel, template, receiver);

        protected virtual IEnumerable<IIntranetActivity> GetAllActivities()
        {
            var allBulletins = _bulletinsService.GetAll().Cast<IIntranetActivity>();
            //todo uncomment when News and Events will be done
			//var allNews = _newsService.GetAll().Cast<IIntranetActivity>();
			//var allEvents = _eventsService.GetAll().Cast<IIntranetActivity>();

			return allBulletins; /*.Concat(allNews).Concat(allEvents);*/
        }
    }
}