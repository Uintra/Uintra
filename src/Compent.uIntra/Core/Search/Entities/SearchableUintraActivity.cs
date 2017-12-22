using System.Collections.Generic;
using System.Linq;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Entities
{
    public class SearchableUintraActivity : SearchableActivity
    {
        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}
