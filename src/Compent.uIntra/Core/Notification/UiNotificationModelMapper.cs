using System;
using System.Linq;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using static uIntra.Notification.Constants.TokensConstants;

namespace Compent.uIntra.Core.Notification
{
    public class UiNotificationModelMapper : INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage>
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public UiNotificationModelMapper(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public UiNotificationMessage Map(INotifierDataValue notifierData, UiNotifierTemplate template, IIntranetUser receiver)
        {
            var message = new UiNotificationMessage
            {
                ReceiverId = receiver.Id
            };
            (string, string)[] tokens;
            switch (notifierData)
            {
                case ActivityNotifierDataModel model:
                    message.NotificationType = model.NotificationType;
                    message.Url = model.Url;
                    message.NotifierId = model.NotifierId;
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.Name),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (NotifierFullName, receiver.DisplayedName)
                    };
                    break;
                case ActivityReminderDataModel model:
                    message.NotificationType = model.NotificationType;
                    message.Url = model.Url;
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.Name),
                        (StartDate, model.StartDate.ToShortDateString())
                    };
                    break;
                case CommentNotifierDataModel model:
                    message.NotificationType = model.NotificationType;
                    message.Url = model.Url;
                    message.NotifierId = model.NotifierId;
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName)
                    };
                    break;
                case LikesNotifierDataModel model:
                    message.NotificationType = model.NotificationType;
                    message.Url = model.Url;
                    message.NotifierId = model.NotifierId;
                    tokens = new[]
                    {
                        (ActivityTitle, model.Title),
                        (ActivityType, model.ActivityType.Name),
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (CreatedDate, model.CreatedDate.ToShortDateString())
                    };
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            message.Message = ReplaceTokens(template.Message, tokens);
            return message;
        }

        private string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));
    }
}