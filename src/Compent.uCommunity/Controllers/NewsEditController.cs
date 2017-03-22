using System;
using System.Web.Mvc;
using Compent.uCommunity.Core.Users;
using uCommunity.Core;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Compent.uCommunity.Controllers
{
    public class NewsEditController : RenderMvcController
    {
        public override ActionResult Index(RenderModel renderModel)
        {
            var currentUser = HttpContext.Session?[IntranetConstants.Session.LoggedUserSessionKey];
            if (currentUser == null)
            {
                HttpContext.Session?.Add(IntranetConstants.Session.LoggedUserSessionKey, new IntranetUser { Name = "Current user 2017", Id = Guid.NewGuid() });
            }

            return View(renderModel);
        }
    }
}