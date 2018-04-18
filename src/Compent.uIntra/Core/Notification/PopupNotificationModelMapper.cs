using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using static Uintra.Notification.Constants.TokensConstants;

namespace Compent.Uintra.Core.Notification
{
    public class PopupNotificationModelMapper : INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IProfileLinkProvider _profileLinkProvider;

        public PopupNotificationModelMapper(IIntranetUserService<IIntranetUser> intranetUserService, IProfileLinkProvider profileLinkProvider)
        {
            _intranetUserService = intranetUserService;
            _profileLinkProvider = profileLinkProvider;
        }

        public PopupNotificationMessage Map(INotifierDataValue notifierData, PopupNotifierTemplate template, IIntranetUser receiver)
        {
            var message = new PopupNotificationMessage
            {
                ReceiverId = receiver.Id,
                NotificationType = NotificationTypeEnum.Welcome
            };

            (string, string)[] tokens =
            {
                (FullName, _intranetUserService.Get(receiver.Id).DisplayedName),
                (ProfileLink, _profileLinkProvider.GetProfileLink(receiver.Id))
            };

            message.Message = ReplaceTokens(template.Message, tokens);
            return message;
        }

        private string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));
    }
}