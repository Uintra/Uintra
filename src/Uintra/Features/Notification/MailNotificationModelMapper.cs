﻿using System;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Notification.Entities;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Entities.Base.Mails;
using Uintra.Features.Notification.Models.NotifierTemplates;
using Uintra.Features.Notification.Services;
using Uintra.Infrastructure.ApplicationSettings;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.Helpers;
using static Uintra.Features.Notification.Constants.TokensConstants;

namespace Uintra.Features.Notification
{
    public class MailNotificationModelMapper : INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage>
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public MailNotificationModelMapper(IApplicationSettings applicationSettings, IIntranetMemberService<IntranetMember> intranetMemberService)
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
                        (Url, model.Url.ToString()),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url.ToString())),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName,_intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (NotifierFullName, receiver.DisplayedName)
                    };
                    break;
                case ActivityReminderDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url.ToString()),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url.ToString())),
                        (ActivityType, model.ActivityType.ToString()),
                        (StartDate, model.StartDate.ToShortDateString()),
                        (FullName, receiver.DisplayedName)
                    };
                    break;
                case CommentNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
                        (FullName,_intranetMemberService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case LikesNotifierDataModel model:
                    tokens = new[]
                    {
                        (Url, model.Url.ToString()),
                        (ActivityTitle, HtmlHelper.CreateLink(GetTitle(model.ActivityType, model.Title), model.Url.ToString())),
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
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
                        (FullName, _intranetMemberService.Get(model.ReceiverId).DisplayedName),
                        (TaggedBy, _intranetMemberService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case GroupInvitationDataModel model:
                    tokens = new[]
                    {
                        (Url, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
                        (Title, model.Title),
                        (ActivityTitle, HtmlHelper.CreateLink(model.Title, model.Url.ToString())),
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

        private string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));

        private static string GetTitle(Enum activityType, string title)
            => activityType is IntranetActivityTypeEnum.Social ? title?.StripHtml().TrimByWordEnd(100) : title;

        private void FillNoReplyFromProps(MailBase message)
        {
            message.FromEmail = _applicationSettings.MailNotificationNoReplyEmail;
            message.FromName = _applicationSettings.MailNotificationNoReplyName;
        }
    }
}