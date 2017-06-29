using System.Collections.Generic;
using System.Linq;
using Nest;

namespace uIntra.Search
{
    public class SearchResult<T>
    {
        public IEnumerable<T> Documents { get; set; } = Enumerable.Empty<T>();

        public IReadOnlyDictionary<string, IAggregate> Facets { get; set; } = new Dictionary<string, IAggregate>();

        public long TotalHits { get; set; }
    }
}