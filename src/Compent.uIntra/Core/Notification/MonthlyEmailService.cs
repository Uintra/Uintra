using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Notification.Mails;
using Uintra.Bulletins;
using Uintra.Core.Activity;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Exceptions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.News;
using Uintra.Notification;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.Notification
{
    public class MonthlyEmailService: MonthlyEmailServiceBase
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IEventsService<EventBase> _eventsService;
        private readonly INewsService<NewsBase> _newsService;
        private readonly IUserTagRelationService _userTagService;
        private readonly IActivityLinkService _activityLinkService;

        public MonthlyEmailService(IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IExceptionLogger logger,
            IApplicationSettings applicationSettings,
            IBulletinsService<BulletinBase> bulletinsService,
            IEventsService<EventBase> eventsService,
            INewsService<NewsBase> newsService,
            IUserTagRelationService userTagService,
            IActivityLinkService activityLinkService) 
            : base(mailService, intranetUserService, logger, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            _eventsService = eventsService;
            _newsService = newsService;
            _userTagService = userTagService;
            _activityLinkService = activityLinkService;
        }

        protected virtual IEnumerable<Guid> GetUserTags(Guid userId)
        {
            return _userTagService.GetForEntity(userId);
        }


        protected override IEnumerable<(IIntranetActivity activity, string detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId)
        {
            var allActivities = GetAllActivities()
                .Select(activity => (activity, activityTagIds: _userTagService.GetForEntity(activity.Id)));

            var userTagIds = _userTagService
                .GetForEntity(userId)
                .ToList();

            var result = allActivities
                .Where(pair => userTagIds.Intersect(pair.activityTagIds).Any())
                .Select(pair => (pair.activity, detailsLink: _activityLinkService.GetLinks(pair.activity.Id).Details));

            return result;
        }

        protected override T GetMonthlyMailModel<T>(string userActivities, IIntranetUser user)
        {
            var result = base.GetMonthlyMailModel<MonthlyMail>(userActivities, user);
            return (T)(object)result;
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