using System;
using System.Linq;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Entities;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Extensions;
using static Uintra20.Features.Notification.Constants.TokensConstants;

namespace Uintra20.Features.Notification
{
    public class DesktopNotificationModelMapper : INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public DesktopNotificationModelMapper(IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }

        public DesktopNotificationMessage Map(INotifierDataValue notifierData, DesktopNotifierTemplate template, IIntranetMember receiver)
        {
            var message = new DesktopNotificationMessage();
            (string, string)[] tokens;
            switch (notifierData)
            {
                case ActivityNotifierDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName, _intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (NotifierFullName, receiver.DisplayedName),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;
                case ActivityReminderDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.ToString()),
                        (StartDate, model.StartDate.ToShortDateString()),
                        (FullName, receiver.DisplayedName),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;
                case CommentNotifierDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (FullName, _intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;
                case LikesNotifierDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.ToString()),
                        (FullName, _intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (CreatedDate, model.CreatedDate.ToShortDateString()),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;
                case UserMentionNotifierDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (FullName, _intranetMemberService.Get(model.ReceiverId).DisplayedName),
                        (TaggedBy, _intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;

                case GroupInvitationDataModel model:
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (FullName, _intranetMemberService.Get(model.ReceiverId).DisplayedName),
                        (TaggedBy, _intranetMemberService.Get(model.NotifierId).DisplayedName),
                        (Title, model.Title),
                        (Url, model.Url.ToString()),
                        (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
                    };
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            message.Title = ReplaceTokens(template.Title, tokens).StripHtml();
            message.Message = ReplaceTokens(template.Message, tokens).StripHtml();

            return message;
        }

        //public async Task<DesktopNotificationMessage> MapAsync(INotifierDataValue notifierData, DesktopNotifierTemplate template, IIntranetMember receiver)
        //{
        //    var message = new DesktopNotificationMessage();
        //    (string, string)[] tokens;
        //    switch (notifierData)
        //    {
        //        case ActivityNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (FullName, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (NotifierFullName, receiver.DisplayedName),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;
        //        case ActivityReminderDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (StartDate, model.StartDate.ToShortDateString()),
        //                (FullName, receiver.DisplayedName),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;
        //        case CommentNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (FullName, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;
        //        case LikesNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (ActivityType, model.ActivityType.ToString()),
        //                (FullName, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (CreatedDate, model.CreatedDate.ToShortDateString()),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;
        //        case UserMentionNotifierDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (FullName, (await _intranetMemberService.GetAsync(model.ReceiverId)).DisplayedName),
        //                (TaggedBy, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;

        //        case GroupInvitationDataModel model:
        //            tokens = new[]
        //            {
        //                (ActivityTitle, model.Title),
        //                (FullName, (await _intranetMemberService.GetAsync(model.ReceiverId)).DisplayedName),
        //                (TaggedBy, (await _intranetMemberService.GetAsync(model.NotifierId)).DisplayedName),
        //                (Title, model.Title),
        //                (Url, model.Url),
        //                (NotificationType, model.NotificationType.ToString().SplitOnUpperCaseLetters())
        //            };
        //            break;
        //        default:
        //            throw new IndexOutOfRangeException();
        //    }

        //    message.Title = ReplaceTokens(template.Title, tokens).StripHtml();
        //    message.Message = ReplaceTokens(template.Message, tokens).StripHtml();

        //    return message;
        //}

        private string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source ?? string.Empty, (acc, pair) => acc.Replace(pair.token, pair.value));
    }
}