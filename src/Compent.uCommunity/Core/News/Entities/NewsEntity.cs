using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using uCommunity.CentralFeed;
using uCommunity.Comments;
using uCommunity.Likes;
using uCommunity.News;
using uCommunity.Subscribe;

namespace Compent.uCommunity.Core.News.Entities
{
    public class NewsEntity : NewsBase, ICentralFeedItem, ICommentable, ILikeable, ISubscribable
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::uCommunity.Subscribe.Subscribe> Subscribers { get; set; }
    }
}