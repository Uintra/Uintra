using uIntra.Core.Activity;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Subscribe
{
    public class CustomSubscribeService : SubscribeService
    {
        public CustomSubscribeService(ISqlRepository<global::uIntra.Subscribe.Subscribe> subscribeRepository)
            : base(subscribeRepository)
        {
        }

        public override bool HasNotification(IIntranetType type)
        {
            return type.Id == (int)IntranetActivityTypeEnum.Events || type.Id == (int)IntranetActivityTypeEnum.News;
        }
    }
}