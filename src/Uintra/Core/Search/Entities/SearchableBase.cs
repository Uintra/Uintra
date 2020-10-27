using Uintra.Features.Links.Models;

namespace Uintra.Core.Search.Entities
{
    public class SearchableBase
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public UintraLinkModel Url { get; set; }

        public int Type { get; set; }
    }
}