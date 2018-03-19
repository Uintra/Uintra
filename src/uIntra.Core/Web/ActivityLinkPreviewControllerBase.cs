using System.Web.Mvc;
using Uintra.Core.LinkPreview;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class ActivityLinkPreviewControllerBase : SurfaceController
    {
        protected virtual string LinkPreviewViewPath { get; set; } = "~/App_Plugins/Core/LinkPreview/LinkPreviewView.cshtml";
        protected virtual string EditLinkPreviewViewPath { get; set; } = "~/App_Plugins/Core/LinkPreview/EditLinkPreviewView.cshtml";

        public ActionResult LinkPreview(LinkPreviewViewModel model)
        {
            if (model == null)
            {
                return new EmptyResult();
            }
            return PartialView(LinkPreviewViewPath, model);
        }

        public ActionResult EditLinkPreview(LinkPreviewViewModel model)
        {
            if (model == null)
            {
                return new EmptyResult();
            }
            return PartialView(EditLinkPreviewViewPath, model);
        }
    }
}
