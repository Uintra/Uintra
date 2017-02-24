using System.Collections.Generic;

namespace uCommunity.Subscribe.App_Plugins.Subscribe
{
    public interface ISubscribable
    {
        IEnumerable<Sql.Subscribe> Subscribers { get; set; }
    }
}