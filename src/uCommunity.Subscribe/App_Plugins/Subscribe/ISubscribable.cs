using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;

namespace uCommunity.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Subscribe> Subscribers { get; set; }

        IntranetActivityTypeEnum Type { get; set; }
    }
}