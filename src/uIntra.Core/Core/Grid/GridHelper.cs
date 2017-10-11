using System.Collections.Generic;
using ClientDependency.Core;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Grid
{
    public class GridHelper : IGridHelper
    {
        public IEnumerable<dynamic> GetValues(IPublishedContent content, string alias)
        {
            dynamic grid = GetGrid(content);

            if (grid == null)
                yield break;

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
                                && control.editor.view != null 
                                && control.editor.alias == alias)
                            {
                                yield return control.value;
                            }
                        }
                    }
                }
            }
        }

        private static dynamic GetGrid(IPublishedContent content)
        {
            return content.GetProperty("grid")?.GetValue<dynamic>();
        }

        public T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey)
        {
            var properties = GetValues(content, contentKey);
            if (properties == null)
            {
                return default(T);
            }
            var propertiesDictionary = ((JToken) properties).ToDictionary();
            object property;
            if (propertiesDictionary.TryGetValue(propertyKey, out property))
            {
                var typedResult = ((JToken) property).ToObject<T>();
                return typedResult;
            }
            return default(T);
        }
    }
}