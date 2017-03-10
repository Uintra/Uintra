using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Extentions;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Core.Controls.LightboxGalery
{
    public class LightboxGaleryController: SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;

        public LightboxGaleryController(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public ActionResult RenderGalery(string mediaIds)
        {
            var result = Enumerable.Empty<LightboxGaleryViewModel>();

            if (mediaIds.IsNotNullOrEmpty())
            {
                var ids = mediaIds.ToIntCollection();
                var medias = _umbracoHelper.TypedMedia(ids).ToList();
                result = medias.Map<IEnumerable<LightboxGaleryViewModel>>().OrderBy(s => s.Type);
            }

            return View("~/App_Plugins/Core/Controls/LightBoxGalery/LightboxGalery.cshtml", result);
        }
    }
}