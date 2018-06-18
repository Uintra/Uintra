using System.Collections.Generic;
using System.Linq;
using Uintra.Search;

namespace Compent.Uintra.Core.Search.Entities
{
    public class SearchableUintraActivity : SearchableActivity, ISearchibleTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}
