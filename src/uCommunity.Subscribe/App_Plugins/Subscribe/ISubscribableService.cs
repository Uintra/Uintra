using System;

namespace uCommunity.Subscribe.App_Plugins.Subscribe
{
    public interface ISubscribableService
    {
        Sql.Subscribe Subscribe(Guid userId, Guid activityId);

        Sql.Subscribe UnSubscribe(Guid userId, Guid activityId);
    }
}