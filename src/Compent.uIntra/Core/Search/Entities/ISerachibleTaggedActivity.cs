using System.Collections.Generic;

namespace Compent.uIntra.Core.Search.Entities
{
    public interface ISearchibleTaggedActivity
    {
        IEnumerable<string> UserTagNames { get; set; }
    }
}