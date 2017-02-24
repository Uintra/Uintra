using System;
using uCommunity.Core.App_Plugins.Core.Activity;

namespace uCommunity.Subscribe.App_Plugins.Subscribe.Model
{
    public class SubscribeViewModel
    {
        public Guid? Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsSubscribed { get; set; }

        public bool HasNotification { get; set; }

        public bool IsNotificationDisabled { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }
    }
}