using System.Collections.Generic;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Entities
{
    public class SearchableUintraContent : SearchableContent,  ISearchibleTaggedActivity
    {
        public IEnumerable<string> UserTagNames { get; set; }

        public bool TagsHighlighted { get; set; }
    }
}