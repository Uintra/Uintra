using System.Collections.Generic;

namespace Uintra20.Core.Search.Entities
{
    public class SearchableContent : SearchableBase
    {
        public IEnumerable<SearchablePanel> Panels { get; set; }
    }
}