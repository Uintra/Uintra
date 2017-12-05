using System;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Core.Services;
using static uIntra.Notification.Constants.TokensConstants;

namespace Compent.uIntra.Core.Notification
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
                         (ActivityTitle, GetHtmlLink(model.Title, model.Url)),
                        (ActivityType, model.ActivityType.Name),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName)
                    };

                    break;
                case ActivityReminderDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, GetHtmlLink(model.Title, model.Url)),
                        (ActivityType, model.ActivityType.Name),
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
                        (ActivityTitle, GetHtmlLink(model.Title, model.Url)),
                        (ActivityType, model.ActivityType.Name),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (CreatedDate, model.CreatedDate.ToShortDateString())
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
    }
}