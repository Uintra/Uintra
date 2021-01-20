using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Search.Entities
{
    public class SearchableActivity : SearchableBase, ISearchableTaggedActivity
    {
        public string Description { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }

        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();

        public bool TagsHighlighted { get; set; }
    }
}
