using System.Web.Mvc;
using uIntra.Core.Extensions;
using uIntra.Core.Location;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityLocationControllerBase : SurfaceController
    {
        protected virtual string ActivityLocationEditViewPath { get; } = "~/App_Plugins/Core/ActivityLocation/Edit/EditView.cshtml";

        public virtual ActionResult Edit(ActivityLocation location)
        {
            var model = location.Map<ActivityLocationEditModel>();
            return PartialView(ActivityLocationEditViewPath, model);
        }
    }
}
