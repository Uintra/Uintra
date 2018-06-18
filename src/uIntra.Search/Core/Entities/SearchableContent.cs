using System.Collections.Generic;

namespace Uintra.Search
{
    public class SearchableContent : SearchableBase
    {
        public IEnumerable<string> PanelContent { get; set; }
        public IEnumerable<string> PanelTitle { get; set; }
    }
}