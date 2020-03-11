using System.Collections.Generic;

namespace Uintra20.Features.Search.Entities
{
    public interface ISearchableTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}