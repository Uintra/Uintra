using System;
using uIntra.Core.Activity;

namespace uIntra.Events
{
    public class EventViewModel : IntranetActivityViewModelBase
    {
        public Guid? CreatorId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Media { get; set; }
        public bool CanSubscribe { get; set; }
        public string SubscribeNotes { get; set; }
    }
}