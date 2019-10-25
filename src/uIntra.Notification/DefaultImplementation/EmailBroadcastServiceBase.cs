using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uintra.Core.Activity;
using Uintra.Core.Exceptions;
using Uintra.Core.User;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    public abstract class EmailBroadcastServiceBase : IEmailBroadcastService
    {
        private readonly IMailService _mailService;
        private readonly IExceptionLogger _logger;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly NotificationSettingsService _notificationSettingsService;

        protected EmailBroadcastServiceBase(IMailService mailService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IExceptionLogger logger,
            NotificationSettingsService notificationSettingsService)
        {
            _mailService = mailService;
            _intranetMemberService = intranetMemberService;
            _logger = logger;
            _notificationSettingsService = notificationSettingsService;
        }

        public abstract void IsBroadcastable();

        public abstract IEnumerable<(IIntranetActivity activity, string detailsLink)> GetUserActivitiesFilteredByUserTags(Guid userId);

        public abstract MailBase GetMailModel(IIntranetMember receiver, BroadcastMailModel model, EmailNotifierTemplate template);

        public void Broadcast()
        {
            var allUsers = _intranetMemberService.GetAll();

            var monthlyMails = allUsers
                .Select(user => user.Id.Pipe(GetUserActivitiesFilteredByUserTags).Pipe(userActivities => TryGetBroadcastMail(userActivities, user)))
                .ToList();

            var identity = new ActivityEventIdentity(
                    CommunicationTypeEnum.CommunicationSettings,
                    NotificationTypeEnum.MonthlyMail)
                .AddNotifierIdentity(NotifierTypeEnum.EmailNotifier);

            var settings = _notificationSettingsService.Get<EmailNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;

            foreach (var monthlyMail in monthlyMails)
            {
                monthlyMail.Do(some: mail =>
                {
                    var mailModel = GetMailModel(mail.user, mail.monthlyMail, settings.Template);
                    try
                    {
                        _mailService.SendMailByTypeAndDay(
                            mailModel,
                            mail.user.Email,
                            DateTime.UtcNow,
                            NotificationTypeEnum.MonthlyMail);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex);
                    }
                });
            }
        }

        public (IIntranetMember user, BroadcastMailModel monthlyMail)? TryGetBroadcastMail(
            IEnumerable<(IIntranetActivity activity, string detailsLink)> activities,
            IIntranetMember member)
        {
            var activityList = activities.AsList();

            if (activityList.Any())
            {
                var activityListString = GetActivityListString(activityList);

                var monthlyMail = new BroadcastMailModel
                {
                    ActivityList = activityListString
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
            return activities
                .Aggregate(
                    new StringBuilder(),
                    (builder, activity) =>
                        builder.AppendLine($"<a href='{activity.link}'>{activity.activity.Title}</a></br>"))
                .ToString();
        }
    }
}
