using System;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Uintra.Core.Helpers;

namespace Uintra.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotFoundActivityAttribute : ActionFilterAttribute
    {
        private const string ActivityIdParameterName = "id";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid? activityId = null;
            if (filterContext.ActionParameters.TryGetValue(ActivityIdParameterName, out var obj))
            {
                activityId = obj as Guid?;
            }

            var activityServices =
                DependencyResolver.Current.GetServices<IIntranetActivityService<IIntranetActivity>>();

            var activity = activityServices.Select(service => service.Get(activityId.GetValueOrDefault()))
                .FirstOrDefault(a => a != null);

            if (activity == null || activity.IsHidden)
            {
                TransferRequestHelper.ToErrorPage(filterContext);
            }
        }
    }
}