using Umbraco.Core.Models;

namespace uIntra.Core.Grid
{
    public interface IGridHelper
    {
        dynamic GetValue(IPublishedContent content, string alias);
    }
}
