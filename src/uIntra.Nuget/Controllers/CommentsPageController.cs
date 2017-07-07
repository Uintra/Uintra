using System.Web.Mvc;
using Compent.uIntra.Core.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.uIntra.Controllers
{
    public class CommentsPageController : RenderMvcController
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public CommentsPageController(IUmbracoContentHelper umbracoContentHelper)
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