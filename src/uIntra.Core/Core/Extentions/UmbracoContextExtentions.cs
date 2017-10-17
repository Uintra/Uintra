using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Extensions
{
    public static class UmbracoContextExtentions
    {
        public static IPublishedContent GetCurrentPage(this UmbracoContext context)
        {
            return context.PublishedContentRequest.PublishedContent;
        }
    }
}