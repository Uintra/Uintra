using System;
using Newtonsoft.Json;
using uCommunity.CentralFeed;
using uCommunity.News;

namespace Compent.uCommunity.Core.News
{
    public class News : NewsModelBase, ICentralFeedItem
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;

        public News()
        {
            
        }
        /*public News(NewsModelBase parentBase)
        {
            Creator = parentBase.Creator;
            UmbracoCreatorId = parentBase.UmbracoCreatorId;
            CreatorId = parentBase.CreatorId;
            MediaIds = parentBase.MediaIds;
            PublishDate = parentBase.PublishDate;
        }*/
    }
}