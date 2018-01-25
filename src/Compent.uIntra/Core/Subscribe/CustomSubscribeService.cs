using System;
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

        public override bool HasNotification(Enum type)
        {
            return type is IntranetActivityTypeEnum.Events || type is IntranetActivityTypeEnum.News;
        }
    }
}