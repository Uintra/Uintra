using System.Net;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.Helpers
{
    public static class TransferRequestHelper
    {
        private static readonly IDocumentTypeAliasProvider AliasProvider;
        private static readonly UmbracoHelper UmbracoHelper;

        static TransferRequestHelper()
        {
            UmbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            AliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();
        }

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

        private static IPublishedContent GetErrorPage()
        {
            var homePage = AliasProvider.GetHomePage();

            var errorPage = AliasProvider.GetErrorPage();

            var xpath = XPathHelper.GetXpath(homePage, errorPage);

            return UmbracoHelper.TypedContentSingleAtXPath(xpath);
        }
    }
}