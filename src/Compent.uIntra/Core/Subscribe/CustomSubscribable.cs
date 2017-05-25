using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;
using uCommunity.Subscribe;

namespace Compent.uCommunity.Core.Subscribe
{
    public class CustomSubscribable : ISubscribable
    {
        public Guid Id { get; set; }

        public IEnumerable<global::uCommunity.Subscribe.Subscribe> Subscribers { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }
    }
}