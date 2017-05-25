using System.Web.Mvc;
using uIntra.Core.ApplicationSettings;
using uIntra.Core.Controls.PinActivity;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class PinActivityControllerBase : SurfaceController
    {
        protected virtual string PinActivityViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityPinView.cshtml";

        private readonly IApplicationSettings _applicationSettings;

        protected PinActivityControllerBase(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public virtual ActionResult PinActivity(bool isPinned, int pinDays)
        {
            return PartialView(PinActivityViewPath,
                new PinActivityModel
                {
                    RangeStart = _applicationSettings.PinDaysRangeStart,
                    RangeEnd = _applicationSettings.PinDaysRangeEnd,
                    PinDays = pinDays,
                    IsPinned = isPinned
                });
        }

    }
}
