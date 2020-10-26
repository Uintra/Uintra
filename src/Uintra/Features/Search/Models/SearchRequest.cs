
using System.Web.Mvc;

namespace Uintra.Features.Search.Models
{
    public class SearchRequest
    {
        [AllowHtml]
        public string Query { get; set; }
    }
}
