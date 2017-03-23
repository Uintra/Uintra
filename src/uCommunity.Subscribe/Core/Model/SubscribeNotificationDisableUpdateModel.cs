using System;
using uCommunity.Core.Activity;

namespace uCommunity.Subscribe
{
    public class SubscribeNotificationDisableUpdateModel
    {
        public Guid Id { get; set; }

        public bool NewValue { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }
    }
}