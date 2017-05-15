using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.Events
{
    public class EventsOverviewItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Teaser { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string MediaIds { get; set; }

        public bool CanSubscribe { get; set; }

        public IntranetActivityItemHeaderViewModel HeaderInfo { get; set; }

        public bool IsPinned { get; set; }        
    }
}