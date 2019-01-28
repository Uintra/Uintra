using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Uintra.Search
{
    public class SearchTextQuery
    {
        public string Text { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string OrderingString { get; set; }

        public IEnumerable<int> SearchableTypeIds { get; set; } = Enumerable.Empty<int>();

        public bool OnlyPinned { get; set; }

        public bool ApplyHighlights { get; set; }

        public Option<Guid> GroupId { get; set; } = None;
    }
}