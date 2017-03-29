using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using uCommunity.CentralFeed;
using uCommunity.Comments;
using uCommunity.Likes;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Entities
{
    public class News : NewsModelBase, ICentralFeedItem, ICommentable, ILikeable
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        public News()
        {
            
        }
    }
}