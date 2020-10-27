using System.Collections.Generic;

namespace Uintra.Core.Search.Entities
{
    public interface ISearchableTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}