using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UBaseline.Core.Node;
using UBaseline.Shared.PageNotFoundPage;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Infrastructure.Helpers
{
    public static class TransferRequestHelper
    {
        public static void ToErrorPage(ActionExecutingContext context)
        {
            context.Result = new HttpNotFoundResult();
        }

        public static void ToForbiddenPage(ActionExecutingContext context)
        {
            context.HttpContext.Response.StatusCode = HttpStatusCode.Forbidden.ToInt();
            context.HttpContext.Response.End();
        }

        public static void ToErrorPage()
        {
            HttpContext.Current.Response.StatusCode = HttpStatusCode.NotFound.ToInt();
            HttpContext.Current.Response.End();
        }

        private static PageNotFoundPageModel GetErrorPage()
        {
            return DependencyResolver.Current.GetService<INodeModelService>().AsEnumerable().OfType<PageNotFoundPageModel>().First();
        }
    }
}