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

        public virtual ActionResult PinActivity(bool isEdit)
        {
            return PartialView(PinActivityViewPath,
                new PinActivityModel
                {
                    RangeStart = applicationSettings.PinDaysRangeStart,
                    RangeEnd = applicationSettings.PinDaysRangeEnd,
                    IsEditMode = isEdit
                });
        }

    }
}
