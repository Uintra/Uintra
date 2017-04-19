using Umbraco.Core.Models;

namespace Compent.uCommunity.Core.Helpers
{
    public interface IUmbracoContentHelper
    {
        bool IsContentAvailable(IPublishedContent publishedContent);
    }
}