using Uintra.Core.Media;
using Uintra.Panels.Web;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class ContentPanelController : ContentPanelControllerBase
    {
        public ContentPanelController(IImageHelper imageHelper, UmbracoHelper umbracoHelper)
            : base(imageHelper, umbracoHelper)
        {
        }
    }
}