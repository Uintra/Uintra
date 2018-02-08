using System.Web.Mvc;
using uIntra.Core.Location;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityLocationControllerBase : SurfaceController
    {
        protected virtual string LocationEditViewPath { get; } = "~/App_Plugins/Core/ActivityLocation/Edit/EditView.cshtml";
        protected virtual string LocationHeaderLinkViewPath { get; } = "~/App_Plugins/Core/ActivityLocation/HeaderLink/View.cshtml";

        private ActivityLocationEditModel EmptyEditModel => new ActivityLocationEditModel();

        public virtual ActionResult Edit(ActivityLocationEditModel location)
        {
            var model = location ?? EmptyEditModel;
            return PartialView(LocationEditViewPath, model);
        }

        public virtual ActionResult HeaderLink(ActivityLocation location)
        {
            return PartialView(LocationHeaderLinkViewPath, location);
        }
    }
}
