using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Core.Search.Entities
{
    public class SearchableContent : SearchableBase
    {
        public IEnumerable<SearchablePanel> Panels { get; set; }=Enumerable.Empty<SearchablePanel>();
    }
}