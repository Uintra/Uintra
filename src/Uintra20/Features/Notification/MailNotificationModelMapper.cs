using System;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Features.Notification.Entities;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;
using static Uintra20.Features.Notification.Constants.TokensConstants;

namespace Uintra20.Features.Notification
{
    public class MailNotificationModelMapper : INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public MailNotificationModelMapper(IApplicationSettings applicationSettings, IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _applicationSettings = applicationSettings;
            _intranetMemberService = intranetMemberService;
        }

        public EmailNotificationMessage Map(INotifierDataValue notifierData, EmailNotifierTemplate template, IIntranetMember receiver)
        {
            var message = new EmailNotificationMessage();
            FillNoReplyFromProps(message);

            (string, string)[] tokens;

            switch (notifierData)
            {
                case ActivityNotifierDataModel model:
                    tokens = new[]
                     {
                        (Url, model.Url),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName,_intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (NotifierFullName, receiver.DisplayedName)
                    };
                    break;
                case ActivityReminderDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (StartDate, model.StartDate.ToShortDateString()),
                        (FullName, receiver.DisplayedName)
                    };
                    break;
                case CommentNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (FullName,_intranetMemberService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case LikesNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName,_intranetMemberService.Get(model.NotifierId).DisplayedName),
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
                case UserMentionNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (FullName, _intranetMemberService.Get(model.ReceiverId).DisplayedName),
                        (TaggedBy, _intranetMemberService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case GroupInvitationDataModel model:
                    tokens = new[]
                    {
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (Title, model.Title),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
                        (FullName, _intranetMemberService.Get(model.ReceiverId).DisplayedName),
                        (TaggedBy, _intranetMemberService.Get(model.NotifierId).DisplayedName)
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

        //public async Task<EmailNotificationMessage> MapAsync(INotifierDataValue notifierData, EmailNotifierTemplate template, IIntranetMember receiver)
        //{
        //    var message = new EmailNotificationMessage();
        //    FillNoReplyFromProps(message);

        //    (string, string)[] tokens;

        //    switch (notifierData)
        //    {
        //        case ActivityNotifierDataModel model:
        //            tokens = new[]
        //             {
        //                (Url, model.Url),
        //                (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (FullName,(await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (NotifierFullName, receiver.DisplayedName)
        //            };
        //            break;
        //        case ActivityReminderDataModel model:
        //            tokens = new[]
        //            {
        //                (Url, model.Url),
        //                (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (StartDate, model.StartDate.ToShortDateString()),
        //                (FullName, receiver.DisplayedName)
        //            };
        //            break;
        //        case CommentNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (FullName,(await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName)
        //            };
        //            break;
        //        case LikesNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (Url, model.Url),
        //                (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url)),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (FullName,(await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (CreatedDate, model.CreatedDate.ToShortDateString())
        //            };
        //            break;
        //        case MonthlyMailDataModel model:
        //            tokens = new[]
        //            {
        //                (FullName, receiver.DisplayedName),
        //                (ActivityList, model.ActivityList)
        //            };
        //            break;
        //        case UserMentionNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (FullName, (await _intranetMemberService.GetAsync(model.ReceiverId)).DisplayedName),
        //                (TaggedBy, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName)
        //            };
        //            break;
        //        case GroupInvitationDataModel model:
        //            tokens = new[]
        //            {
        //                (Url, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (Title, model.Title),
        //                (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url)),
        //                (FullName, (await _intranetMemberService.GetAsync(model.ReceiverId)).DisplayedName),
        //                (TaggedBy, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName)
        //            };
        //            break;
        //        default:
        //            throw new IndexOutOfRangeException();
        //    }

        //    message.Body = ReplaceTokens(template.Body, tokens);
        //    message.Subject = ReplaceTokens(template.Subject, tokens);
        //    message.Recipients = new MailRecipient { Name = receiver.DisplayedName, Email = receiver.Email }.ToListOfOne();
        //    return message;
        //}

        public string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));

        private static string GetTitle(Enum activityType, string title)
            => activityType is IntranetActivityTypeEnum.Bulletins ? title?.StripHtml().TrimByWordEnd(100) : title;

        private void FillNoReplyFromProps(MailBase message)
        {
            message.FromEmail = _applicationSettings.MailNotificationNoReplyEmail;
            message.FromName = _applicationSettings.MailNotificationNoReplyName;
        }
    }
}