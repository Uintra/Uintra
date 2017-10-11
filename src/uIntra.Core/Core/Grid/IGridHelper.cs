using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.Core.Grid
{
    public interface IGridHelper
    {
        IEnumerable<dynamic> GetValues(IPublishedContent content, string alias);
        T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey);
    }
}