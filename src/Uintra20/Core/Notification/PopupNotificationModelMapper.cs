using System.Linq;
using Uintra20.Core.Helpers;
using Uintra20.Core.Localization;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Core.User;
using static Uintra20.Core.Notification.Constants.TokensConstants;

namespace Uintra20.Core.Notification
{
    public class PopupNotificationModelMapper : INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>
    {
        private const string ProfileLinkTitle = "PopupNotification.ProfileLink.Title";

        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;

        public PopupNotificationModelMapper(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IIntranetLocalizationService localizationService,
            IIntranetUserContentProvider intranetUserContentProvider)
        {
            _intranetMemberService = intranetMemberService;
            _localizationService = localizationService;
            _intranetUserContentProvider = intranetUserContentProvider;
        }

        public PopupNotificationMessage Map(INotifierDataValue notifierData, PopupNotifierTemplate template, IIntranetMember receiver)
        {
            var message = new PopupNotificationMessage
            {
                ReceiverId = receiver.Id,
                NotificationType = NotificationTypeEnum.Welcome
            };

            (string, string)[] tokens =
            {
                (FullName, _intranetMemberService.Get(receiver.Id).DisplayedName),
                (ProfileLink, HtmlHelper.CreateLink(_localizationService.Translate(ProfileLinkTitle), _intranetUserContentProvider.GetEditPage().Url))
            };
            message.Message = ReplaceTokens(template.Message, tokens);
            return message;
        }

        private string ReplaceTokens(string source, params (string token, string value)[] replacePairs) =>
            replacePairs
                .Aggregate(source, (acc, pair) => acc.Replace(pair.token, pair.value));
    }
}