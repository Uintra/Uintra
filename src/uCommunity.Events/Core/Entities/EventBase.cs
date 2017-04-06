using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity.Entities;

namespace uCommunity.Events
{
    public class EventBase : IntranetActivityModelBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<int> MediaIds { get; set; }
        public bool CanSubscribe { get; set; }
        public Guid CreatorId { get; set; }
        public int? UmbracoCreatorId { get; set; }

        public EventBase()
        {
            MediaIds = Enumerable.Empty<int>();
        }
    }
}
