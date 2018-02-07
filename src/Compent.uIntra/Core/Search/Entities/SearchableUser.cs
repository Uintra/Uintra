using System.Collections.Generic;
using System.Linq;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Entities
{
    public class SearchableUser : SearchableBase, ISearchibleTaggedActivity
    {
        public string Photo { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}