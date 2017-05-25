using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.UmbracoModelsBuilders;
using uCommunity.Core;
using uCommunity.Core.ApplicationSettings;
using uCommunity.Core.Media;
using uCommunity.Users.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(IMemberService memberService,
           UmbracoHelper umbracoHelper,
           IMediaHelper mediaHelper,
           IApplicationSettings applicationSettings)
            : base(memberService, umbracoHelper, mediaHelper, applicationSettings)
        {
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