using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Localization;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Services;
using Uintra20.Infrastructure.Helpers;
using static Uintra20.Features.Notification.Constants.TokensConstants;

namespace Uintra20.Features.Notification
{
    public class PopupNotificationModelMapper : INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage>
    {
        private const string ProfileLinkTitle = "PopupNotification.ProfileLink.Title";

        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;

        public PopupNotificationModelMapper(
            IIntranetMemberService<IntranetMember> intranetMemberService,
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

        public async Task<PopupNotificationMessage> MapAsync(INotifierDataValue notifierData, PopupNotifierTemplate template, IIntranetMember receiver)
        {
            var message = new PopupNotificationMessage
            {
                ReceiverId = receiver.Id,
                NotificationType = NotificationTypeEnum.Welcome
            };

            (string, string)[] tokens =
            {
                //(FullName, (await _intranetMemberService.GetAsync(receiver.Id)).DisplayedName),
                (FullName, (_intranetMemberService.Get(receiver.Id)).DisplayedName),
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