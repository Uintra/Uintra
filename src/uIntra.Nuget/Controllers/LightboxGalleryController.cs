using uIntra.Core.Controls.LightboxGallery;
using Umbraco.Web;

namespace uIntra.Controllers
{
    public class LightboxGalleryController : LightboxGalleryControllerBase
    {
        public LightboxGalleryController(UmbracoHelper umbracoHelper)
            : base(umbracoHelper)
        {
        }
    }
}