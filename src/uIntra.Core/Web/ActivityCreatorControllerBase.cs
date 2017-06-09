using System.Web.Mvc;
using uIntra.Core.User;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class ActivityCreatorControllerBase : SurfaceController
    {
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Core/Activity/ActivityCreatorEdit.cshtml";

        public virtual ActionResult Edit(ICanEditCreatorCreateEditModel editModel)
        {
            return PartialView(EditViewPath, editModel);
        }
    }
}
