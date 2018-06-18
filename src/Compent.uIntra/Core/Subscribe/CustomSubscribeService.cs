using System;
using Uintra.Core.Activity;
using Uintra.Core.Persistence;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.Subscribe
{
    public class CustomSubscribeService : SubscribeService
    {
        public CustomSubscribeService(ISqlRepository<global::Uintra.Subscribe.Subscribe> subscribeRepository)
            : base(subscribeRepository)
        {
        }

        public override bool HasNotification(Enum type) => 
            type is IntranetActivityTypeEnum.Events || type is IntranetActivityTypeEnum.News;
    }
}