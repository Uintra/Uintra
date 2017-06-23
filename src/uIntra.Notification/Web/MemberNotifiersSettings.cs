using System;
using System.Linq;
using System.Web.Http;
using uIntra.Core.Extentions;
using uIntra.Notification.Configuration;
using Umbraco.Web.WebApi;

namespace uIntra.Notification.Web
{
    public class MemberNotifiersSettingsApiController : UmbracoApiController
    {
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;

        public MemberNotifiersSettingsApiController(IMemberNotifiersSettingsService memberNotifiersSettingsService)
        {
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
        }

        [HttpPost]
        public void Update(Guid memberId, NotifierTypeEnum type, bool isEnabled)
        {
             _memberNotifiersSettingsService.SetForMember(memberId, type,isEnabled);
        }
    }
}
