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
            if (renderModel.Content.Name=="Error")
            {
                HttpContext.Response.StatusCode = HttpStatusCode.NotFound.ToInt();
            }

            if (renderModel.Content.Name == "Forbidden")
            {
                HttpContext.Response.StatusCode = HttpStatusCode.Forbidden.ToInt();
            }

            return View(string.Empty, renderModel.Content);
        }
    }
}