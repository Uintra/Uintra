using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Uintra.Core.UmbracoIpAccess
{
    public class UmbracoIpAccessApiFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var umbracoIpAccessValidator = DependencyResolver.Current.GetService<IUmbracoIpAccessValidator>();

            var controllerAssembly = filterContext.ControllerContext.ControllerDescriptor.ControllerType.Assembly;
            umbracoIpAccessValidator.Validate(HttpContext.Current, controllerAssembly);
        }
    }
}