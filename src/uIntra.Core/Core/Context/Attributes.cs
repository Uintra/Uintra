using System.Web.Mvc;

namespace Uintra.Core.Context
{
    public class TrackContextAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is ContextedController contextController)
            {
                 contextController.UpdateViewData();
            }
        }
    }

    public class ContextActionAttribute : ActionFilterAttribute
    {
        private readonly ContextBuildActionType _type;

        public ContextActionAttribute(ContextBuildActionType actionType)
        {
            _type = actionType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is ContextController contextController)
            {
                contextController.ContextBuildActionType = _type;
            }
        }
    }
}
