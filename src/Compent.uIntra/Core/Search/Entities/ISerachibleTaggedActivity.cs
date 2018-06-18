using System.Collections.Generic;

namespace Compent.Uintra.Core.Search.Entities
{
    public interface ISearchibleTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}