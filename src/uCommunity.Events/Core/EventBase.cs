using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uCommunity.Core.Activity.Entities;
using uCommunity.Core.User;

namespace uCommunity.Events
{
    public class EventBase : IntranetActivity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public bool CanSubscribe { get; set; }
        public Guid CreatorId { get; set; }
        public int? UmbracoCreatorId { get; set; }

        [JsonIgnore]
        public DateTime PublishDate => StartDate;
        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
    }
}
