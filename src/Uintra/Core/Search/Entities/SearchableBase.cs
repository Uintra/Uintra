using Compent.Shared.Search.Contract;
using Uintra.Features.Links.Models;

namespace Uintra.Core.Search.Entities
{
    public class SearchableBase : ISearchDocument
    {
        public string Id { get; set; }

        public string Culture { get; set; }

        public string Title { get; set; }

        public UintraLinkModel Url { get; set; }

        public int Type { get; set; }
    }
}