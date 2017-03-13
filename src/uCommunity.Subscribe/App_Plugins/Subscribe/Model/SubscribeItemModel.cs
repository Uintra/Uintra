using System;

namespace uCommunity.Subscribe.Model
{
    public class SubscribeItemModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public bool IsNotificationDisabled { get; set; }
    }
}