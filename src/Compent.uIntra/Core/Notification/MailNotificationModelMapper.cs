using System;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using static Uintra.Notification.Constants.TokensConstants;

namespace Compent.Uintra.Core.Notification
{
    public class MailNotificationModelMapper : INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public MailNotificationModelMapper(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public EmailNotificationMessage Map(INotifierDataValue notifierData, EmailNotifierTemplate template, IIntranetUser receiver)
        {
            var message = new EmailNotificationMessage();
            (string, string)[] tokens;

            switch (notifierData)
            {
                case ActivityNotifierDataModel model:
                    tokens = new[]
                     {
                        (Url, model.Url),
                        (ActivityTitle, GetHtmlLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (NotifierFullName, receiver.DisplayedName)
                    };
                    break;
                case ActivityReminderDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, GetHtmlLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (StartDate, model.StartDate.ToShortDateString())
                    };
                    break;
                case CommentNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, GetHtmlLink(model.Title, model.Url)),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case LikesNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, GetHtmlLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (CreatedDate, model.CreatedDate.ToShortDateString())
                    };
                    break;
                case MonthlyMailDataModel model:
                    tokens = new[]
                    {
                        (FullName, receiver.DisplayedName),
                        (ActivityList, model.ActivityList)
                    };
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            message.Body = ReplaceTokens(template.Body, tokens);
            message.Subject = ReplaceTokens(template.Subject, tokens);
            message.Recipients = new MailRecipient { Name = receiver.DisplayedName, Email = receiver.Email }.ToListOfOne();
            return message;
        }

        public string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));

        private static string GetHtmlLink(string title, string url) => $"<a href=\"{url.ToAbsoluteUrl()}\">{title}</a>";

        private static string GetTitle(Enum activityType, string title)
            => activityType is IntranetActivityTypeEnum.Bulletins ? title?.StripHtml().TrimByWordEnd(100) : title;
    }
}