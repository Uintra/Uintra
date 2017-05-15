using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface IHaveTags
    {
        Guid Id { get; }

        IEnumerable<string> Tags { get; set; }
    }
}
