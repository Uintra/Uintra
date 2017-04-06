using System;
using Newtonsoft.Json;
using uCommunity.CentralFeed;
using uCommunity.Events;

namespace Compent.uCommunity.Core.Events
{
    public class Event : EventModelBase, ICentralFeedItem
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
    }
}