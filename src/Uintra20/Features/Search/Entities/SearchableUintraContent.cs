using System.Collections.Generic;

namespace Uintra20.Features.Search.Entities
{
    public class SearchableUintraContent : SearchableContent,  ISearchableTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; }

        public bool TagsHighlighted { get; set; }
    }
}