using uCommunity.Core.Activity;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Subscribe;

namespace Compent.uIntra.Core.Subscribe
{
    public class CustomSubscribeService : SubscribeService
    {
        public CustomSubscribeService(ISqlRepository<global::uCommunity.Subscribe.Subscribe> subscribeRepository)
            : base(subscribeRepository)
        {
        }

        public override bool HasNotification(IntranetActivityTypeEnum type)
        {
            return type == IntranetActivityTypeEnum.Events || type == IntranetActivityTypeEnum.News;
        }
    }
}