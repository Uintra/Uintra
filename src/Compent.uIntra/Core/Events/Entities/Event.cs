using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Events;
using Uintra.Groups;
using Uintra.Likes;
using Uintra.Notification.Base;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.Events
{
    public class Event : EventBase, IFeedItem, ICommentable, ILikeable, ISubscribable, ISubscribeSettings, IReminderable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<CommentModel> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::Uintra.Subscribe.Subscribe> Subscribers { get; set; }

        [JsonIgnore]
        public bool CanSubscribe { get; set; }
        [JsonIgnore]
        public string SubscribeNotes { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}