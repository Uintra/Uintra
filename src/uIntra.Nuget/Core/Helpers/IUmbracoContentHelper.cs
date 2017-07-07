using Umbraco.Core.Models;

namespace Compent.uIntra.Core.Helpers
{
    public interface IUmbracoContentHelper
    {
        bool IsContentAvailable(IPublishedContent publishedContent);
    }
}