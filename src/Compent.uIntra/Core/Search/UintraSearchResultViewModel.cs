using System.Collections.Generic;
using System.Linq;
using uIntra.Search;

namespace Compent.uIntra.Core.Search
{
    public class UintraSearchResultViewModel : SearchResultViewModel
    {
        public string Photo { get; set; }

        public string Email { get; set; }

        public bool TagsHighlighted { get; set; }

        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();
    }
}