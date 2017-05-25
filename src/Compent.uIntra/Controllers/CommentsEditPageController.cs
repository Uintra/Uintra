using System.Web.Mvc;
using Compent.uCommunity.Core.Helpers;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.uCommunity.Controllers
{
    public class CommentsEditPageController : RenderMvcController
    {
        private readonly IUmbracoContentHelper _umbracoContentHelper;

        public CommentsEditPageController(IUmbracoContentHelper umbracoContentHelper)
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