using System;
using System.Web.Mvc;
using Uintra.Core.Context;
using Uintra.Core.PagePromotion;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.Web
{
    public abstract class GridControllerBase : ContextController
    {
        public override Enum ControllerContextType { get; } = ContextType.ContentPage;

        protected virtual string GridViewPath { get; } = "~/App_Plugins/Core/Grid/Grid.cshtml";

        protected GridControllerBase(IContextTypeProvider contextTypeProvider)
            : base(contextTypeProvider)
        {
        }

        public virtual PartialViewResult Render(IPublishedContent content)
        {
            AddEntityIdentityForContext(
                entityId: content.GetKey(),
                controllerContextType: PagePromotionHelper.IsPromoted(content) ? ContextType.PagePromotion : ControllerContextType);

            return PartialView(GridViewPath, content);
        }
    }
}