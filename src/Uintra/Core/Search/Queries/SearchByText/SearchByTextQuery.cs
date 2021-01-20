using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Search.Queries
{
    public class SearchByTextQuery : UBaseline.Search.Core.SearchByTextQuery
    {
        public string OrderingString { get; set; }

        public IEnumerable<int> SearchableTypeIds { get; set; } = Enumerable.Empty<int>();

        public bool OnlyPinned { get; set; }
    }
}