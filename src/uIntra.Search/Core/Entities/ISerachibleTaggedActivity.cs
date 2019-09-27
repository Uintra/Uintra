using System.Collections.Generic;

namespace Uintra.Search
{
    public interface ISearchableTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}