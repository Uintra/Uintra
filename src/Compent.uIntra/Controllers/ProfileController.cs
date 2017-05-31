using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Media;
using uIntra.Users.Web;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        private UmbracoHelper _umbracoHelper;

        public ProfileController(IMemberService memberService,
           UmbracoHelper umbracoHelper,
           IMediaHelper mediaHelper,
           IApplicationSettings applicationSettings)
            : base(memberService, umbracoHelper, mediaHelper, applicationSettings)
        {
            _umbracoHelper = umbracoHelper;
        }

        public ActionResult ToProfilePage()
        {
            var profilePage = _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, ProfilePage.ModelTypeAlias));
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