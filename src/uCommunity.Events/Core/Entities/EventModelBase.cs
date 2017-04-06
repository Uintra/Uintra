using System;
using Newtonsoft.Json;
using uCommunity.Core.User;

namespace uCommunity.Events
{
    public class EventModelBase : EventBase, IHaveCreator
    {
        [JsonIgnore]
        public DateTime PublishDate => StartDate;
        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
    }
}