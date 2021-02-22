using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Search.Entities
{
    public class SearchableContent : SearchableBase
    {
        public string AggregatedTextFromPanels { get; set; }
        public IEnumerable<SearchablePanel> Panels { get; set; }=Enumerable.Empty<SearchablePanel>();
    }
}