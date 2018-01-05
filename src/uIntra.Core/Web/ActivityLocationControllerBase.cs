using System.Web.Mvc;
using uIntra.Core.Extensions;
using uIntra.Core.Location;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityLocationControllerBase : SurfaceController
    {
        protected virtual string ActivityLocationEditViewPath { get; } = "~/App_Plugins/Core/ActivityLocation/Edit/EditView.cshtml";

        private ActivityLocationEditModel EmptyEditModel => new ActivityLocationEditModel();

        public virtual ActionResult Edit(ActivityLocationEditModel location)
        {
            var model = location ?? EmptyEditModel;
            return PartialView(ActivityLocationEditViewPath, model);
        }
    }
}
