using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.Events
{
    public class EventsOverviewItemModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string MediaIds { get; set; }

        public bool CanSubscribe { get; set; }

        public IntranetActivityHeaderModel HeaderInfo { get; set; }
    }
}