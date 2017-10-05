using System.Web.Mvc;
using Compent.uIntra.Core.Helpers;
using uIntra.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.uIntra.Controllers
{
    public class SubscribePageController : RenderMvcController
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public SubscribePageController(IUmbracoContentHelper umbracoContentHelper)
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