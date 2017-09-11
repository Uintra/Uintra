using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Events;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.Notification.Base;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Events
{
    public class Event : EventBase, IFeedItem, ICommentable, ILikeable, ISubscribable, IReminderable, IGroupable
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}