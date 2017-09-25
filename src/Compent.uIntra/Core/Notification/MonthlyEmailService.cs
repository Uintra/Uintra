using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Notification.Mails;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Exceptions;
using uIntra.Core.Extentions;
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
        private readonly IExceptionLogger _logger;

        public MonthlyEmailService(IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IExceptionLogger logger,
            IApplicationSettings applicationSettings,
            IBulletinsService<BulletinBase> bulletinsService,
            IEventsService<EventBase> eventsService,
            INewsService<NewsBase> newsService,
            TagsService tagsService) : base(mailService, intranetUserService, logger, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            _eventsService = eventsService;
            _newsService = newsService;
            _tagsService = tagsService;
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

            var allUserActivities = GetAllUserActivities(userId);
            var userTags = GetUserTags(userId);

            if (userTags != null && userTags.Any())
            {
                foreach (var activity in allUserActivities)
                {
                    var activityTags = GetActivityTags(activity.Id);
                    if (activityTags != null && activityTags.Any(a => userTags.FirstOrDefault(tag => tag.Id == a.Id) != null))
                    {
                        string activityDetailsPageUrl = GetActivityDetailsUrl(activity);
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

        protected virtual IEnumerable<IIntranetActivity> GetAllUserActivities(Guid userId)
        {
            var allBulletins = _bulletinsService.GetAll().Where(b => b.CreatorId == userId).Cast<IIntranetActivity>();
            var allNews = _newsService.GetAll().Where(n => n.CreatorId == userId).Cast<IIntranetActivity>();
            var allEvents = _eventsService.GetAll().Where(e => e.CreatorId == userId).Cast<IIntranetActivity>();

            return allBulletins.Concat(allNews).Concat(allEvents);
        }

        protected virtual string GetActivityDetailsUrl(IIntranetActivity activity)
        {
            switch (activity.Type.Id)
            {
                case (int)IntranetActivityTypeEnum.Bulletins:
                    {

                        return _bulletinsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
                    }

                case (int)IntranetActivityTypeEnum.News:
                    {

                        return _newsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
                    }

                case (int)IntranetActivityTypeEnum.Events:
                    {

                        return _eventsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }
    }
}