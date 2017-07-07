using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Events;
using uIntra.Likes;
using uIntra.Notification.Base;
using uIntra.Subscribe;

namespace uIntra.Core.Events
{
    public class Event : EventBase, ICentralFeedItem, ICommentable, ILikeable, ISubscribable, IReminderable
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }
    }
}