using System.Collections.Generic;

namespace uCommunity.Subscribe.App_Plugins.Subscribe
{
    public interface ISubscribe
    {
        IEnumerable<Sql.Subscribe> Subscribers { get; set; }
    }
}