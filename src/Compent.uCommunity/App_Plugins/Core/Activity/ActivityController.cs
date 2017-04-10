using System.Web.Mvc;
using uCommunity.Core.Activity.Models;
using Umbraco.Web.Mvc;

namespace uCommunity.Core
{
    public class ActivityController : SurfaceController
    {
        public ActionResult ActivityDetailsHeader(IntranetActivityDetailsHeaderViewModel header)
        {
            return PartialView("~/App_Plugins/Core/Activity/ActivityDetailsHeader.cshtml", header);
        }

        public ActionResult ActivityItemHeader(IntranetActivityItemHeaderViewModel header)
        {
            return PartialView("~/App_Plugins/Core/Activity/ActivityItemHeader.cshtml", header);
        }
    }
}