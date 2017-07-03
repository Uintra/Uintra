using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Users;
using uIntra.Users.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;

        public ProfileController(IMemberService memberService,
           UmbracoHelper umbracoHelper,
           IMediaHelper mediaHelper,
           IApplicationSettings applicationSettings,
           IIntranetUserService<IntranetUser> intranetUserService,
           IIntranetUserContentHelper intranetUserContentHelper,
           IMemberNotifiersSettingsService memberNotifiersSettingsService)
            : base(memberService, umbracoHelper, mediaHelper, applicationSettings, intranetUserService, memberNotifiersSettingsService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserContentHelper = intranetUserContentHelper;
        }

        public ActionResult EditPage()
        {
            var profilePage = _intranetUserContentHelper.GetEditPage();
            return Redirect(profilePage.Url);
        }

        public override MediaSettings GetMediaSettings()
        {
            var media = _umbracoHelper.TypedMediaAtRoot();
            var id =
                media.Single(m => m.GetPropertyValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) ==
                        MediaFolderTypeEnum.MembersContent).Id;

            return new MediaSettings
            {
                MediaRootId = id
            };
        }
    }
}