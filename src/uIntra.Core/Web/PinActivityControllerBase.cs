using System;
using System.Web.Mvc;
using uIntra.Core.Controls.PinActivity;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class PinActivityControllerBase : SurfaceController
    {
        protected virtual string PinActivityViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityPinView.cshtml";

        public virtual ActionResult PinActivity(bool isPinned, DateTime? endPinDate)
        {
            return PartialView(PinActivityViewPath,
                new PinActivityModel
                {
                    IsPinned = isPinned,
                    EndPinDate = endPinDate ?? DateTime.Now
                });
        }
    }
}
