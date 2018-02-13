using System;
using System.Collections.Generic;

namespace Uintra.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Subscribe> Subscribers { get; set; }

        Enum Type { get; set; }
    }
}