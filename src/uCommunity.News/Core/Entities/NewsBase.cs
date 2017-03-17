using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity.Entities;

namespace uCommunity.News
{
    public class NewsBase : IntranetActivityModelBase
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<int> MediaIds { get; set; }
        public DateTime PublishDate { get; set; }
      
        public NewsBase()
        {
            MediaIds = Enumerable.Empty<int>();
        }        
    }
}