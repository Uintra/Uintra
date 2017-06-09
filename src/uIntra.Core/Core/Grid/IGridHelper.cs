using Umbraco.Core.Models;

namespace uIntra.Core.Grid
{
    public interface IGridHelper
    {
        dynamic GetValue(IPublishedContent content, string alias);
        T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey);
    }
}
