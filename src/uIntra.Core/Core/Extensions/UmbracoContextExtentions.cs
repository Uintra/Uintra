using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.Extensions
{
    public static class UmbracoContextExtensions
    {
        public static IPublishedContent GetCurrentPage(this UmbracoContext context)
        {
            return context.PublishedContentRequest.PublishedContent;
        }
    }
}