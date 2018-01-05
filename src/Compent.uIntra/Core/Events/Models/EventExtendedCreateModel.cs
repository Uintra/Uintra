using uIntra.Events;

namespace Compent.uIntra.Core.Events
{
    public class EventExtendedCreateModel : EventCreateModel
    {
        public bool CanSubscribe { get; set; }

        public string SubscribeNotes { get; set; }

        public bool CanEditSubscribe { get; set; }
    }
}