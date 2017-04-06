using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Events
{
    public class EventsOverviewModel
    {
        public IEnumerable<EventsOverviewItemModel> Items { get; set; }

        public EventsOverviewModel()
        {
            Items = Enumerable.Empty<EventsOverviewItemModel>();
        }
    }
}