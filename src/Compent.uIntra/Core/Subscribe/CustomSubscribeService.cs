using uIntra.Core.Activity;
using uIntra.Core.Persistence;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Subscribe
{
    public class CustomSubscribeService : SubscribeService
    {
        public CustomSubscribeService(ISqlRepository<global::uIntra.Subscribe.Subscribe> subscribeRepository)
            : base(subscribeRepository)
        {
        }

        public override bool HasNotification(IntranetActivityTypeEnum type)
        {
            return type == IntranetActivityTypeEnum.Events || type == IntranetActivityTypeEnum.News;
        }
    }
}