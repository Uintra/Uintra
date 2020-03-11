using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Search.Entities
{
    public class SearchableUintraActivity : SearchableActivity, ISearchableTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}
