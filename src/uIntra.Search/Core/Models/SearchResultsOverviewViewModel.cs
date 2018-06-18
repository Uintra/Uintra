using System.Collections.Generic;
using System.Linq;

namespace uIntra.Search
{
    public class SearchResultsOverviewViewModel
    {
        public IEnumerable<SearchResultViewModel> Results { get; set; } = Enumerable.Empty<SearchResultViewModel>();

        public IEnumerable<SearchFilterItemViewModel> FilterItems { get; set; } = Enumerable.Empty<SearchFilterItemViewModel>();

        public string Query { get; set; }

        public int ResultsCount { get; set; }

        public bool BlockScrolling { get; set; }

        public string AllTypesPlaceholder { get; set; }
    }
}