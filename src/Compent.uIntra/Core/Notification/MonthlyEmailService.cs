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

        public MonthlyEmailService(IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IExceptionLogger logger,
            IApplicationSettings applicationSettings,
            IBulletinsService<BulletinBase> bulletinsService,
            IEventsService<EventBase> eventsService,
            INewsService<NewsBase> newsService) 
            : base(mailService, intranetUserService, logger, applicationSettings)
        {
            _bulletinsService = bulletinsService;
            _eventsService = eventsService;
            _newsService = newsService;
        }

        protected override List<Tuple<IIntranetActivity, string>> GetUserActivitiesFilteredByUserTags(Guid userId)
        {
            throw new NotImplementedException();
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