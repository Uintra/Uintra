using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Search.Entities
{
    public class SearchableUintraActivity : SearchableActivity, ISearchableTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}
