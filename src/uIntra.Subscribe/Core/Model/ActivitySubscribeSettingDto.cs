using System;

namespace uIntra.Subscribe
{
    public class ActivitySubscribeSettingDto
    {
        public Guid ActivityId { get; set; }

        public bool CanSubscribe { get; set; }

        public string SubscribeNotes { get; set; }
    }
}