using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Users.Web;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;


        public ProfileController(
            IMediaHelper mediaHelper,
            IApplicationSettings applicationSettings,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService, 
            UmbracoHelper umbracoHelper,
            IIntranetUserContentProvider intranetUserContentProvider) 
            : base(mediaHelper, applicationSettings, intranetUserService, memberNotifiersSettingsService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserContentProvider = intranetUserContentProvider;
        }

        public ActionResult EditPage()
        {
            var profilePage = _intranetUserContentProvider.GetEditPage();
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