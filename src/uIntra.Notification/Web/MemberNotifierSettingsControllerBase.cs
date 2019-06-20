using System.Web.Http;
using Uintra.Core.User;
using Uintra.Notification.Configuration;
using Umbraco.Web.WebApi;

namespace Uintra.Notification.Web
{
    public class MemberNotifierSettingsController : UmbracoApiController
    {
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public MemberNotifierSettingsController(
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _intranetMemberService = intranetMemberService;
        }

        [HttpPost]
        public void Update(NotifierTypeEnum type, bool isEnabled)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            _memberNotifiersSettingsService.SetForMember(currentMember.Id, type, isEnabled);
        }
    }
}