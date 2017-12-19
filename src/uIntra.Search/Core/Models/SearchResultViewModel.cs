using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Search
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> PanelContent { get; set; } = Enumerable.Empty<string>();

        public string Url { get; set; }

        public string Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }
    }
}