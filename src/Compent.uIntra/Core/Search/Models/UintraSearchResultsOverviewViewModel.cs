using System.Collections.Generic;
using System.Linq;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Models
{
    public class UintraSearchResultsOverviewViewModel
    {
        public IEnumerable<UintraSearchResultViewModel> Results { get; set; } = Enumerable.Empty<UintraSearchResultViewModel>();

        public IEnumerable<SearchFilterItemViewModel> FilterItems { get; set; } = Enumerable.Empty<SearchFilterItemViewModel>();

        public string Query { get; set; }

        public int ResultsCount { get; set; }

        public string AllTypesPlaceholder { get; set; }
    }
}