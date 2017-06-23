using System.Collections.Generic;
using System.Linq;
using Nest;

namespace uIntra.Search.Core
{
    public class SearchResult<T>
    {
        public IEnumerable<T> Documents { get; set; }

        public IReadOnlyDictionary<string, IAggregate> Facets { get; set; }

        public long TotalHits { get; set; }

        public SearchResult()
        {
            Documents = Enumerable.Empty<T>();
            Facets = new Dictionary<string, IAggregate>();
        }
    }
}