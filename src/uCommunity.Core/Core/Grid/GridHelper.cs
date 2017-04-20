using Umbraco.Core.Models;
using Umbraco.Web;

namespace uCommunity.Core.Grid
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
    }
}
