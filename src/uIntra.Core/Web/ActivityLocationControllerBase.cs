using System.Web.Mvc;
using uIntra.Core.Location;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityLocationControllerBase : SurfaceController
    {
        protected virtual string ActivityLocationEditViewPath { get; } = "~/App_Plugins/Core/ActivityLocation/Edit/EditView.cshtml";

        public virtual ActionResult Edit(string address, string shortAddress)
        {
            var model = new ActivityLocationEditModel
            {
                Address = address,
                ShortAddress = shortAddress
            };

            return PartialView(ActivityLocationEditViewPath, model);
        }
    }
}
