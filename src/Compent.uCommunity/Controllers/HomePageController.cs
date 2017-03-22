using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.uCommunity.Controllers
{
    public class HomePageController : RenderMvcController
    {
        public override ActionResult Index(RenderModel renderModel)
        {
            return View();
        }
    }
}