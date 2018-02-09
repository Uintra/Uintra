using System.Collections.Generic;
using System.Linq;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Models
{
    public class UintraSearchResultsOverviewViewModel
    {
        public IEnumerable<UintraSearchResultViewModel> Results { get; set; } = Enumerable.Empty<UintraSearchResultViewModel>();

        public IEnumerable<SearchFilterItemViewModel> FilterItems { get; set; } = Enumerable.Empty<SearchFilterItemViewModel>();

        public string Query { get; set; }

        public int ResultsCount { get; set; }

        public bool BlockScrolling { get; set; }

        public string AllTypesPlaceholder { get; set; }
    }
}