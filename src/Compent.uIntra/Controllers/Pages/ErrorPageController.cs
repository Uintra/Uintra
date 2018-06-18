using System.Net;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.Uintra.Controllers.Pages
{
    public class ErrorPageController : RenderMvcController
    {
        public override ActionResult Index(RenderModel renderModel)
        {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.ToInt();

            return View(string.Empty, renderModel.Content);
        }
    }
}