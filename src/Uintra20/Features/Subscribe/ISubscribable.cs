using System;
using System.Collections.Generic;

namespace Uintra20.Features.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Sql.Subscribe> Subscribers { get; set; }

        Enum Type { get; set; }
    }
}
