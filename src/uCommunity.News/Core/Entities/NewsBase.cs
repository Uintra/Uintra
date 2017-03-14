using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uCommunity.Core.Activity.Entities;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public abstract class NewsBase : IntranetActivityBase, IHasCreator<IntranetUserBase>
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<int> MediaIds { get; set; }
        public DateTime PublishDate { get; set; }
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IntranetUserBase Creator { get; set; }

        public NewsBase()
        {
            MediaIds = Enumerable.Empty<int>();
        }        
    }
}