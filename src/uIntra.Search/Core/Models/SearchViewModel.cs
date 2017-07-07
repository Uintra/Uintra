using System.Collections.Generic;
using System.Linq;
using uIntra.Search.Web;

namespace uIntra.Search
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        public IEnumerable<SearchFilterItemViewModel> FilterItems { get; set; } = Enumerable.Empty<SearchFilterItemViewModel>();
    }
}