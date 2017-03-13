using System;
using System.Collections.Generic;

namespace uCommunity.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Subscribe> Subscribers { get; set; }
    }
}