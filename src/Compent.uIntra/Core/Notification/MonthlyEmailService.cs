using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Bulletins;
using Compent.uIntra.Core.Events;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Core.Exceptions;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.News;
using uIntra.Notification;
using uIntra.Tagging;

namespace Compent.uIntra.Core.Notification
{
    public class MonthlyEmailService//: MonthlyEmailServiceBase
    {
        //private readonly IBulletinsService<Bulletin> _bulletinsService;
        //private readonly IEventsService<Event> _eventsService;
        //private readonly INewsService<News.Entities.News> _newsService;
        //private readonly TagsService _tagsService;
        //private readonly IExceptionLogger _logger;

        //public MonthlyEmailService(IMailService mailService, 
        //    IIntranetUserService<IIntranetUser> intranetUserService,
        //    IExceptionLogger logger,
        //    IBulletinsService<Bulletin> bulletinsService,
        //    IEventsService<Event> eventsService, 
        //    INewsService<News.Entities.News> newsService, 
        //    TagsService tagsService) : base(mailService, intranetUserService, logger)
        //{
        //    _bulletinsService = bulletinsService;
        //    _eventsService = eventsService;
        //    _newsService = newsService;
        //    _tagsService = tagsService;
        //}

        //protected virtual List<Tag> GetUserTags(Guid userId)
        //{
        //    return new List<Tag>();
        //}

        //protected override List<Tuple<IIntranetActivity, string>> GetUserActivitiesFilteredByUserTags(Guid userId)
        //{
        //    var allActivitiesRelatedToUserTags = new List<Tuple<IIntranetActivity, string>>();

        //    var allUserActivities = GetAllUserActivities(userId);
        //    var userTags = GetUserTags(userId);

        //    if (userTags != null && userTags.Any())
        //    {
        //        foreach (var activity in allUserActivities)
        //        {
        //            var activityTags = _tagsService.GetAllForActivity(activity.Id);
        //            if (activityTags != null && activityTags.Any(a => userTags.FirstOrDefault(tag => tag.Id == a.Id) != null))
        //            {
        //                string activityDetailsPageUrl = GetActivityDetailsUrl(activity);
        //                var activityData = Tuple.Create(activity, activityDetailsPageUrl);
        //                allActivitiesRelatedToUserTags.Add(activityData);
        //            }
        //        }
        //    }

        //    return allActivitiesRelatedToUserTags;
        //}

        //protected virtual IEnumerable<IIntranetActivity> GetAllUserActivities(Guid userId)
        //{
        //    var allBulletins = _bulletinsService.GetAll().Where(b => b.CreatorId == userId).Cast<IIntranetActivity>();
        //    var allNews = _newsService.GetAll().Where(n => n.CreatorId == userId).Cast<IIntranetActivity>();
        //    var allEvents = _eventsService.GetAll().Where(e => e.CreatorId == userId).Cast<IIntranetActivity>();

        //    return allBulletins.Concat(allNews).Concat(allEvents);
        //}

        //protected virtual string GetActivityDetailsUrl(IIntranetActivity activity)
        //{
        //    switch (activity.Type.Id)
        //    {
        //        case (int) IntranetActivityTypeEnum.Bulletins:
        //        {

        //            return _bulletinsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
        //        }

        //        case (int) IntranetActivityTypeEnum.News:
        //        {

        //            return _newsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
        //        }

        //        case (int) IntranetActivityTypeEnum.Events:
        //        {

        //            return _eventsService.GetDetailsPage().Url.AddIdParameter(activity.Id);
        //        }
        //        default:
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }
        //}
    }
}