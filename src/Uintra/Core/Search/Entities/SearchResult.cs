using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Search.Contract;

namespace Uintra.Core.Search.Entities
{
    public class SearchResult<T> : Compent.Shared.Search.Contract.SearchResult<T> where T : ISearchDocument
    {
        public IEnumerable<BaseFacet> TypeFacets { get; set; } = Enumerable.Empty<BaseFacet>();
    }
}