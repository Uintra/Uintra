using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uintra.Bulletins;
using Uintra.Core.Activity;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.News;
using Uintra.Notification.Base;
using Uintra.Tagging.UserTags;

namespace Uintra.Notification
{
    public abstract class EmailBroadcastServiceBase<T> 
        : IEmailBroadcastService<T> where T : IMailBroadcast
    {
        private readonly IBulletinsService<BulletinBase> bulletinsService;
        private readonly IEventsService<EventBase> eventsService;
        private readonly INewsService<NewsBase> newsService;
        private readonly IUserTagRelationService userTagService;
        private readonly IActivityLinkService activityLinkService;

        protected EmailBroadcastServiceBase(
            IActivityLinkService activityLinkService, 
            INewsService<NewsBase> newsService, 
            IEventsService<EventBase> eventsService, 
            IBulletinsService<BulletinBase> bulletinsService, 
            IUserTagRelationService userTagService)
        {
            this.activityLinkService = activityLinkService;
            this.newsService = newsService;
            this.eventsService = eventsService;
            this.bulletinsService = bulletinsService;
            this.userTagService = userTagService;
        }

        public abstract void IsBroadcastable();

        public abstract void Broadcast();

        public abstract MailBase GetMailModel(IIntranetMember receiver, BroadcastMailModel model, EmailNotifierTemplate template);

        public IEnumerable<(IIntranetActivity activity, string detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId)
        {
            var allActivities = GetAllActivities()
                .Select(activity => (activity: activity, activityTagIds: userTagService.GetForEntity(activity.Id)));

            var userTagIds = userTagService.GetForEntity(userId);

            var result = allActivities
                .Where(pair => userTagIds.Intersect(pair.activityTagIds).Any())
                .Select(pair => (pair.activity, detailsLink: activityLinkService.GetLinks(pair.activity.Id).Details));

            return result;
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
            Func<StringBuilder, (IIntranetActivity activity, string link), StringBuilder> func = (builder, activity) =>
            {
                return builder.AppendLine($"<a href='{activity.link}'>{activity.activity.Title}</a></br>");
            };

            var stringBuilder = activities.Aggregate(new StringBuilder(), func);

            var result = stringBuilder.ToString();

            return result;
        }

        public virtual IEnumerable<IIntranetActivity> GetAllActivities()
        {
            var allBulletins = bulletinsService.GetAll().Cast<IIntranetActivity>();

            var allNews = newsService.GetAll().Cast<IIntranetActivity>();

            var allEvents = eventsService.GetAll().Cast<IIntranetActivity>();

            return allBulletins.Concat(allNews).Concat(allEvents);
        }
    }
}
