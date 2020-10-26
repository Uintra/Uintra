using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Search.Entities
{
    public class SearchableContent : SearchableBase
    {
        public IEnumerable<SearchablePanel> Panels { get; set; }=Enumerable.Empty<SearchablePanel>();
    }
}