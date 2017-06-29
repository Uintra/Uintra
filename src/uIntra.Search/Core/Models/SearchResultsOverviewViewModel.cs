using System.Collections.Generic;
using System.Linq;

namespace uIntra.Search
{
    public class SearchResultsOverviewViewModel
    {
        public IEnumerable<SearchResultViewModel> Results { get; set; } = Enumerable.Empty<SearchResultViewModel>();

        public string Query { get; set; }

        public int ResultsCount { get; set; }
    }
}