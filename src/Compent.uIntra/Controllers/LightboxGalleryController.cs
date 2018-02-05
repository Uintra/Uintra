using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Links;
using uIntra.Core.Media;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class LightboxGalleryController : LightboxGalleryControllerBase
    {
        public LightboxGalleryController(UmbracoHelper umbracoHelper, IActivityLinkService linkService, IImageHelper imageHelper)
            : base(umbracoHelper, linkService, imageHelper)
        {
        }
    }
}