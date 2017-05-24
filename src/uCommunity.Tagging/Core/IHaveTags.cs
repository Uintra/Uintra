using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface IHaveTags
    {
        Guid Id { get; }

        IEnumerable<Tag> Tags { get; set; }
    }
}
