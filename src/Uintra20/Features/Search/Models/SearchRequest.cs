
using System.Web.Mvc;

namespace Uintra20.Features.Search.Models
{
    public class SearchRequest
    {
        [AllowHtml]
        public string Query { get; set; }
    }
}
