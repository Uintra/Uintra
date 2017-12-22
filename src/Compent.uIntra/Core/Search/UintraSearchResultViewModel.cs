using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Models
{
    public class UintraSearchResultViewModel : SearchResultViewModel
    {
        public string Photo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool TagsHighlighted { get; set; }

        public IEnumerable<string> UserTagNames { get; set; } = Enumerable.Empty<string>();
    }
}