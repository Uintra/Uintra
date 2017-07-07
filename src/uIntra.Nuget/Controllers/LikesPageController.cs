using System.Web.Mvc;
using uIntra.Core.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace uIntra.Controllers
{
    public class LikesPageController : RenderMvcController
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public LikesPageController(IUmbracoContentHelper umbracoContentHelper)
        {
            _umbracoContentHelper = umbracoContentHelper;
        }

        public override ActionResult Index(RenderModel model)
        {
            if (!_umbracoContentHelper.IsContentAvailable(model.Content))
            {
                return HttpNotFound();
            }

            return View(model);
        }
    }
}