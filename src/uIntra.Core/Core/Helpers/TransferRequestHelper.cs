using System.Net;
using System.Web;
using System.Web.Mvc;
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
//            var result = GetErrorPage();

//            if (result != null)
//            {
////                context.Controller.ControllerContext.HttpContext.Server.Transfer(result.Url);
//
//                HttpContext.Current.Response.Redirect(result.Url);
//                return;
//            }

            context.Result = new HttpNotFoundResult();
        }

        public static void ToErrorPage()
        {
//            var result = GetErrorPage();

//            if (result != null)
//            {
//                HttpContext.Current.Server.Transfer(result.Url);
                HttpContext.Current.Response.StatusCode = (int) HttpStatusCode.NotFound;
                HttpContext.Current.Response.End();
//            }

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