using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core;
using uIntra.Notification.Core.Services;
using static uIntra.Notification.Constants.TokensConstants;

namespace Compent.uIntra.Core.Notification
{
    public class UiNotificationModelMapper : INotificationModelMapper<UiNotificationMessage>
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public UiNotificationModelMapper(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public UiNotificationMessage Map(NotifierData notifierData, UiNotifierTemplate template)
        {

            var message = new UiNotificationMessage
            {
                NotificationType = notifierData.NotificationType,
                ReceiverIds = notifierData.ReceiverIds
            };

            switch (notifierData.Value)
            {
                case ActivityNotifierDataModel model:
                    message.Url = model.Url;
                    message.Message = ReplaceTokens(
                        template.Message,
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (ActivityTitle, model.Title));
                    break;
                case ActivityReminderDataModel model:
                    message.Url = model.Url;
                    message.Message = ReplaceTokens(
                        template.Message,
                        (ActivityTitle, model.Title),
                        (StartDate, model.StartDate.ToShortDateString()));
                    break;
                case CommentNotifierDataModel model:
                    message.Url = model.Url;
                    message.Message = ReplaceTokens(
                        template.Message,
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (ActivityTitle, model.Title));
                    break;
                case LikesNotifierDataModel model:
                    message.Url = model.Url;
                    message.Message = ReplaceTokens(
                        template.Message,
                        (FullName, _intranetUserService.Get(model.NotifierId).DisplayedName),
                        (ActivityTitle, model.Title));
                    break;
            }
            return message;
        }

        public string ReplaceTokens(string source, params(string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));

    }
}