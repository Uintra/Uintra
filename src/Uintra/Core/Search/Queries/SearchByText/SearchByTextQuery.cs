using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Search.Contract;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Queries
{
    public class SearchByTextQuery : UBaseline.Search.Core.SearchByTextQuery
    {
        public string OrderingString { get; set; }

        public IEnumerable<int> SearchableTypeIds { get; set; } = Enumerable.Empty<int>();

        public bool OnlyPinned { get; set; }
    }
}