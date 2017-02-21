using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Core.App_Plugins.Core.Extentions
{
    public static class UmbracoContextExtentions
    {
        public static IPublishedContent GetCurrentPage(this UmbracoContext context)
        {
            return context.PublishedContentRequest.PublishedContent;
        }
    }
}