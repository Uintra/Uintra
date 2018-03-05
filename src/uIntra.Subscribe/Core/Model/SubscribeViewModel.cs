using System;

namespace Uintra.Subscribe
{
    public class SubscribeViewModel
    {
        public Guid? Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsSubscribed { get; set; }

        public bool HasNotification { get; set; }

        public bool IsNotificationDisabled { get; set; }
    }
}