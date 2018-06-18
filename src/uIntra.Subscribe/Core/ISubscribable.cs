using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Subscribe> Subscribers { get; set; }

        IIntranetType Type { get; set; }
    }
}