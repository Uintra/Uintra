using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Notification.Mails;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Exceptions;
using uIntra.Core.Links;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.News;
using uIntra.Notification;
using uIntra.Tagging;

namespace Compent.uIntra.Core.Notification
{
    public class MonthlyEmailService: MonthlyEmailServiceBase
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IEventsService<EventBase> _eventsService;
        private readonly INewsService<NewsBase> _newsService;
        private readonly TagsService _tagsService;
        private readonly IActivityLinkService _activityLinkService;

        public MonthlyEmailService(IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IExceptionLogger logger,
            IApplicationSettings applicationSettings,
            IBulletinsService<BulletinBase> bulletinsService,
            IEventsService<EventBase> eventsService,
            INewsService<NewsBase> newsService,
            TagsService tagsService, IActivityLinkService activityLinkService) 
            : base(mailService, intranetUserService, logger, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            _eventsService = eventsService;
            _newsService = newsService;
            _tagsService = tagsService;
            _activityLinkService = activityLinkService;
        }

        protected virtual IEnumerable<Tag> GetUserTags(Guid userId)
        {
            return new List<Tag>();
        }

        protected virtual IEnumerable<Tag> GetActivityTags(Guid activityId)
        {
            return _tagsService.GetAllForActivity(activityId);
        }

        protected override List<Tuple<IIntranetActivity, string>> GetUserActivitiesFilteredByUserTags(Guid userId)
        {
            var allActivitiesRelatedToUserTags = new List<Tuple<IIntranetActivity, string>>();

            var allActivities = GetAllActivities();
            var userTags = GetUserTags(userId);

            if (userTags != null && userTags.Any())
            {
                foreach (var activity in allActivities)
                {
                    var activityTags = GetActivityTags(activity.Id);
                    if (activityTags != null && activityTags.Any(a => userTags.FirstOrDefault(tag => tag.Id == a.Id) != null))
                    {
                        string activityDetailsPageUrl = _activityLinkService.GetLinks(activity.Id).Details;
                        var activityData = Tuple.Create(activity, activityDetailsPageUrl);
                        allActivitiesRelatedToUserTags.Add(activityData);
                    }
                }
            }

            return allActivitiesRelatedToUserTags;
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