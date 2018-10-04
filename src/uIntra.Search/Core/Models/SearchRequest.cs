
using System.Web.Mvc;

namespace Uintra.Search
{
    public class SearchRequest
    {
        [AllowHtml]
        public string Query { get; set; }
    }
}
