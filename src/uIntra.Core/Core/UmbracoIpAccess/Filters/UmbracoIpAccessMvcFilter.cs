using System.Web.Mvc;

namespace Uintra.Core.UmbracoIpAccess
{
    public class UmbracoIpAccessMvcFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var umbracoIpAccessValidator = DependencyResolver.Current.GetService<IUmbracoIpAccessValidator>();

            var assembly = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Assembly;
            umbracoIpAccessValidator.Validate(filterContext.HttpContext.ApplicationInstance.Context, assembly);
        }
    }
}