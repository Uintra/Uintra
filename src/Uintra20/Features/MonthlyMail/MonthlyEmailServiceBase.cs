using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compent.Extensions;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.ApplicationSettings;
using Umbraco.Core.Logging;

namespace Uintra20.Features.MonthlyMail
{
    public abstract class MonthlyEmailServiceBase : IMonthlyEmailService
    {
        private readonly IMailService _mailService;

        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ILogger _logger;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IApplicationSettings _applicationSettings;

        protected MonthlyEmailServiceBase(IMailService mailService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ILogger logger,
            INotificationSettingsService notificationSettingsService,
            IApplicationSettings applicationSettings)
        {
            _mailService = mailService;
            _intranetMemberService = intranetMemberService;
            _logger = logger;

            _notificationSettingsService = notificationSettingsService;
            _applicationSettings = applicationSettings;
        }

        public void CreateAndSendMail()
        {
            var currentDate = DateTime.UtcNow;

            var allUsers = _intranetMemberService.GetAll();
            var monthlyMails = allUsers
                .Select(user =>
                    user.Id.Pipe(GetUserActivitiesFilteredByUserTags)
                        .Pipe(userActivities => TryGetMonthlyMail(userActivities, user)))
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
                    var mailModel = GetMonthlyMailModel(mail.user, mail.monthlyMail, settings.Template);
                    try
                    {
                        _mailService.SendMailByTypeAndDay(
                            mailModel,
                            mail.user.Email,
                            currentDate,
                            NotificationTypeEnum.MonthlyMail);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error<MonthlyEmailServiceBase>(ex);
                    }
                });
            }
        }

        public void ProcessMonthlyEmail()
        {
            if (IsSendingDay())
            {
                CreateAndSendMail();
            }
        }


        protected (IIntranetMember user, MonthlyMailDataModel monthlyMail)? TryGetMonthlyMail(
            IEnumerable<(IIntranetActivity activity, UintraLinkModel detailsLink)> activities,
            IIntranetMember member)
        {
            var activityList = activities.AsList();
            if (activityList.Any())
            {
                var activityListString = GetActivityListString(activityList);
                var monthlyMail = GetMonthlyMailModel(activityListString, member);
                return (member, monthlyMail);
            }
            else
            {
                return default((IIntranetMember user, MonthlyMailDataModel monthlyMail));
            }
        }

        protected abstract IEnumerable<(IIntranetActivity activity, UintraLinkModel detailsLink)>
            GetUserActivitiesFilteredByUserTags(Guid userId);

        protected abstract MailBase GetMonthlyMailModel(IIntranetMember receiver, MonthlyMailDataModel dataModel,
            EmailNotifierTemplate template);

        protected virtual MonthlyMailDataModel GetMonthlyMailModel(string userActivities, IIntranetMember member) =>
            new MonthlyMailDataModel
            {
                ActivityList = userActivities
            };

        protected virtual bool IsSendingDay()
        {
            var currentDate = DateTime.UtcNow;

            return currentDate.Day != _applicationSettings.MonthlyEmailJobDay;
        }

        private string
            GetActivityListString(IEnumerable<(IIntranetActivity activity, UintraLinkModel link)> activities) =>
            activities
                .Aggregate(
                    new StringBuilder(),
                    (builder, activity) =>
                        builder.AppendLine($"<a href='{activity.link}'>{activity.activity.Title}</a></br>"))
                .ToString();
    }
}