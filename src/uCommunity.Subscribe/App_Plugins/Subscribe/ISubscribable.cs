using System;
using System.Collections.Generic;

namespace uCommunity.Subscribe.App_Plugins.Subscribe
{
    public interface ISubscribable
    {
        Guid Id { get; set; }

        IEnumerable<Sql.Subscribe> Subscribers { get; set; }
    }
}