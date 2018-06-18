using System.Collections.Generic;
using System.Linq;
using ClientDependency.Core;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.Core.Grid
{
    public class GridHelper : IGridHelper
    {
        public IEnumerable<(string alias, dynamic value)> GetValues(IPublishedContent content, params string[] aliases)
        {
            dynamic grid = GetGrid(content);

            return IsValidGrid(grid)
                ? GetValues(grid, aliases)
                : Enumerable.Empty<(string, dynamic)>();
        }

        public IEnumerable<(string alias, dynamic value)> GetValues(dynamic grid, params string[] aliases)
        {
            foreach (var section in grid.sections)
            {
                foreach (var row in section.rows)
                {
                    foreach (var area in row.areas)
                    {
                        foreach (var control in area.controls)
                        {
                            if (control != null
                                && control.editor != null
                                //&& control.editor.view != null
                                && aliases.Contains((string)control.editor.alias))
                            {
                                yield return ((string)control.editor.alias, control.value);
                            }
                        }
                    }
                }
            }
        }

        private static dynamic GetGrid(IPublishedContent content) => 
            content.GetProperty("grid")?.GetValue<dynamic>();

        private bool IsValidGrid(object grid) => 
            grid != null && grid is JObject gridJson && gridJson["sections"] != null;


        public T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey)
        {
            var properties = GetValues(content, contentKey);
            if (properties == null)
            {
                return default;
            }
            var propertiesDictionary = ((JToken) properties).ToDictionary();
            if (propertiesDictionary.TryGetValue(propertyKey, out var property))
            {
                var typedResult = ((JToken)property).ToObject<T>();
                return typedResult;
            }
            return default;
        }
    }
}