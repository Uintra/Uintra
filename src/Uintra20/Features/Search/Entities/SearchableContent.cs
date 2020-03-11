using System.Collections.Generic;

namespace Uintra20.Features.Search.Entities
{
    public class SearchableContent : SearchableBase
    {
        public IEnumerable<string> PanelContent { get; set; }
        public IEnumerable<string> PanelTitle { get; set; }
    }
}