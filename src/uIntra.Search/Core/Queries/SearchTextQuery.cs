using System.Collections.Generic;
using System.Linq;

namespace uIntra.Search
{
    public class SearchTextQuery
    {
        public string Text { get; set; }

        public int Take { get; set; }

        public IEnumerable<int> SearchableTypeIds { get; set; } = Enumerable.Empty<int>();

        public bool OnlyPinned { get; set; }

        public bool ApplyHighlights { get; set; }
    }
}