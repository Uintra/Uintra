using System;
using System.Collections.Generic;
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
    public class Event : EventBase, IFeedItem, ICommentable, ILikeable, ISubscribable, ISubscribeSettings, IReminderable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        [JsonIgnore]
        public bool CanSubscribe { get; set; }
        [JsonIgnore]
        public string SubscribeNotes { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}