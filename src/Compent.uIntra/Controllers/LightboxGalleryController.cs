using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Links;
using Uintra.Core.Media;
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