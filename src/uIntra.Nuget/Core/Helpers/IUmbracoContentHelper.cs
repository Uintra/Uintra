using Umbraco.Core.Models;

namespace uIntra.Core.Helpers
{
    public interface IUmbracoContentHelper
    {
        bool IsContentAvailable(IPublishedContent publishedContent);
    }
}