using Compent.Uintra.Controllers.Api;
using System.Web.Mvc;

namespace Compent.Uintra.Core.Sync.Attributes
{
    public class AuthCallbackExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var controller = (AuthCallbackController)filterContext.Controller;
            filterContext.Result = controller.Error(filterContext.Exception);
        }
    }
}