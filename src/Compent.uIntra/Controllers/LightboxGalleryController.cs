using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Links;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class LightboxGalleryController : LightboxGalleryControllerBase
    {
        public LightboxGalleryController(UmbracoHelper umbracoHelper, IActivityLinkService activityLinkService)
            : base(umbracoHelper, activityLinkService)
        {
        }
    }
}