using System.Collections.Generic;
using System.Web.Mvc;

namespace Uintra20.Features.Search.Models
{
    public class SearchFilterModel
    {
        [AllowHtml]
        public string Query { get; set; }
        public int Page { get; set; }
        public IList<int> Types { get; set; } = new List<int>();
        public bool OnlyPinned { get; set; }
    }
}
