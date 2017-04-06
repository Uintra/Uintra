using System;

namespace uCommunity.Events
{
    public class EventEditModel : EventEditModelBase
    {
        public Guid Id { get; set; }

        public bool NotifyAllSubscribers { get; set; }
    }
}