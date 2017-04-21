using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uCommunity.Core.Activity.Entities;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsBase : IntranetActivity, IHaveCreator
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        [JsonIgnore]
        public IIntranetUser Creator { get; set; }
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public DateTime PublishDate { get; set; }

    }
}