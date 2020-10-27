using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Search.Models
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<SearchPanelResultViewModel> Panels { get; set; } = Enumerable.Empty<SearchPanelResultViewModel>();

        public UintraLinkModel Url { get; set; }

        public string Type { get; set; }

        public string PublishedDate { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsPinned { get; set; }

        public bool IsPinActual { get; set; }
    }
}