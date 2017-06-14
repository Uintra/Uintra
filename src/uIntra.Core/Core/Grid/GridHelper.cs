using ClientDependency.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Core.Grid
{
    public class GridHelper : IGridHelper
    {
        public dynamic GetValue(IPublishedContent content, string alias)
        {
            var grid = content?.GetProperty("grid")?.GetValue<dynamic>();

            if (grid == null)
            {
                return null;
            }

            foreach (var section in grid.sections)
            {
                foreach (var row in section.rows)
                {
                    foreach (var area in row.areas)
                    {
                        foreach (var control in area.controls)
                        {
                            if (control != null && control.editor != null && control.editor.view != null && control.editor.alias == alias)
                            {
                                return control.value;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public T GetContentProperty<T>(IPublishedContent content, string contentKey, string propertyKey)
        {
            var properties = GetValue(content, contentKey);
            if (properties == null)
            {
                return default(T);
            }
            var propertiesDictionary = ((JToken) properties).ToDictionary();
            object property;
            if (propertiesDictionary.TryGetValue(propertyKey, out property))
            {
                var typedResult = ((JToken) property).Value<T>();
                return typedResult;
            }
            return default(T);
        }
    }
}