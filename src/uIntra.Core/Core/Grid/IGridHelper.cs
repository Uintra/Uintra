using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.Core.Grid
{
    public interface IGridHelper
    {
        IEnumerable<(string alias, dynamic value)> GetValues(IContentBase content, params string[] aliases);
        IEnumerable<(string alias, dynamic value)> GetValues(IPublishedContent content, params string[] aliases);
        IEnumerable<(string alias, dynamic value)> GetValues(dynamic grid, params string[] aliases);
        T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey);
    }
}