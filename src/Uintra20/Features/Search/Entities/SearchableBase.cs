using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Search.Entities
{
    public class SearchableBase
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public UintraLinkModel Url { get; set; }

        public int Type { get; set; }
    }
}