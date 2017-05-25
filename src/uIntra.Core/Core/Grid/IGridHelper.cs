using Umbraco.Core.Models;

namespace uCommunity.Core.Grid
{
    public interface IGridHelper
    {
        dynamic GetValue(IPublishedContent content, string alias);
    }
}
