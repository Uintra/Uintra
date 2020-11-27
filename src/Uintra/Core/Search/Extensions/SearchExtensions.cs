using System.Collections.Generic;
using System.Linq;
using Nest;
using Uintra.Core.Search.Entities;

namespace Uintra.Core.Search.Extensions
{
    public static class SearchExtensions
    {
        public static IEnumerable<BaseFacet> GetGlobalFacets(this AggregateDictionary facets, string facetName)
        {
            var globalFilter = facets.Global(facetName);
            var items = globalFilter.Terms(facetName).Buckets.Select(bucket => new BaseFacet
                { Name = bucket.Key, Count = bucket.DocCount ?? default(long) });
            return items;
        }
    }
}