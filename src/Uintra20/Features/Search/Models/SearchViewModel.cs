using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Search.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        public IEnumerable<SearchFilterItemViewModel> FilterItems { get; set; } = Enumerable.Empty<SearchFilterItemViewModel>();

        public bool OnlyPinned { get; set; }
    }
}