using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Uintra.Core
{
    public class ViewRenderer
    {
        public string Render(string viewPath, object viewModel)
        {
            var controllerContext = CreateControllerContext();

            using (var writer = new StringWriter())
            {
                var result = ViewEngines.Engines.FindPartialView(controllerContext, viewPath);
                if (result.View == null)
                    throw new Exception($"Email view not found for {viewPath}. Locations searched:{Environment.NewLine} {string.Join(Environment.NewLine, result.SearchedLocations)}");

                var viewContext = new ViewContext(controllerContext, result.View, new ViewDataDictionary(viewModel), new TempDataDictionary(), writer);
                result.View.Render(viewContext, writer);
                result.ViewEngine.ReleaseView(controllerContext, result.View);
                return writer.GetStringBuilder().ToString();
            }
        }

        private static ControllerContext CreateControllerContext()
        {
            // A dummy HttpContextBase that is enough to allow the view to be rendered.
            var httpContext = new HttpContextWrapper(
                new HttpContext(
                    new HttpRequest(string.Empty, UrlRoot(), string.Empty),
                    new HttpResponse(TextWriter.Null)));
            var routeData = new RouteData();
            routeData.Values["controller"] = "Stub";
            var requestContext = new RequestContext(httpContext, routeData);
            var stubController = new StubController();
            var controllerContext = new ControllerContext(requestContext, stubController);
            stubController.ControllerContext = controllerContext;
            return controllerContext;
        }

        private static string UrlRoot()
        {
            var request = HttpContext.Current?.Request;
            return request == null
                ? "http://localhost"
                : request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath;
        }

        private class StubController : Controller { }
    }
}
