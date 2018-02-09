using System.Web.Http;
using Uintra.Core.User;
using Uintra.Notification.Configuration;
using Umbraco.Web.WebApi;

namespace Uintra.Notification.Web
{
    public class MemberNotifierSettingsController : UmbracoApiController
    {
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public MemberNotifierSettingsController(
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _intranetUserService = intranetUserService;
        }

        [HttpPost]
        public void Update(NotifierTypeEnum type, bool isEnabled)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            _memberNotifiersSettingsService.SetForMember(currentUser.Id, type, isEnabled);
        }
    }
}