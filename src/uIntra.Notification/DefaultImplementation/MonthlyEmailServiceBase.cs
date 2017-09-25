using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uIntra.Core.Activity;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Exceptions;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.MailModels;

namespace uIntra.Notification
{
    public abstract class MonthlyEmailServiceBase : IMonthlyEmailService
    {
        private readonly IMailService _mailService;
        private readonly IExceptionLogger _logger;       
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IApplicationSettings _applicationSettings;

        protected MonthlyEmailServiceBase(IMailService mailService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IExceptionLogger logger,
            IApplicationSettings applicationSettings)
        {
            _mailService = mailService;
            _intranetUserService = intranetUserService;
            _logger = logger;
            _applicationSettings = applicationSettings;
        }

        public void SendEmail()
        {
            if (DateTime.Now.Day == _applicationSettings.MonthlyEmailJobDay)
            {
                var users = _intranetUserService.GetAll();
                foreach (var user in users)
                {
                    try
                    {
                        var activities = GetUserActivitiesFilteredByUserTags(user.Id);
                        if (activities.Any())
                        {
                            string activityListString = GetActivityListString(activities);
                            var monthlyMail = GetMonthlyMailModel<MonthlyMailBase>(activityListString, user);
                            _mailService.SendOneTimePerDayMailForSpecialTypeAndDay(monthlyMail, DateTime.Now, NotificationTypeEnum.MonthlyMail);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex);
                    }

                }
            }
        }

        protected abstract List<Tuple<IIntranetActivity, string>> GetUserActivitiesFilteredByUserTags(Guid userId);        

        protected virtual T GetMonthlyMailModel<T>(string userActivities, IIntranetUser user) where T: MonthlyMailBase, new()
        {
            var recipient = new MailRecipient { Email = user.Email, Name = user.DisplayedName };
            return new T
            {
                FullName = user.DisplayedName,
                ActivityList = userActivities,
                Recipients = recipient.ToListOfOne()
            };
        }

        private string GetActivityListString(IEnumerable<Tuple<IIntranetActivity, string>> activities)
        {
            var builder = new StringBuilder();
            foreach (var activity in activities)
            {
                builder.AppendLine($"<a href='{activity.Item2}'>{activity.Item1.Title}</a></br>");
            }
            return builder.ToString();
        }

    }
}
