using System.Web.Mvc;
using uCommunity.Core.ApplicationSettings;
using uCommunity.Core.Controls.PinActivity;
using Umbraco.Web.Mvc;

namespace uCommunity.Core.Web
{
    public abstract class ActivityHeaderControllerBase : SurfaceController
    {
        protected virtual string PinActivityViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityPinView.cshtml";

        private readonly IApplicationSettings applicationSettings;

        protected ActivityHeaderControllerBase(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public virtual ActionResult PinActivity()
        {
            return PartialView(PinActivityViewPath,
                new ActivityPinDaysRangeModel
                {
                    RangeStart = applicationSettings.PinDaysRangeStart,
                    RangeEnd = applicationSettings.PinDaysRangeEnd
                });
        }

    }
}
