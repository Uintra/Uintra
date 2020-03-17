using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Search.Models
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<SearchPanelResultViewModel> Panels { get; set; } = Enumerable.Empty<SearchPanelResultViewModel>();

        public string Url { get; set; }

        public string Type { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }
    }
}