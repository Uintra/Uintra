using System.Collections.Generic;

namespace Uintra20.Core.Search.Entities
{
    public interface ISearchableTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}