using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra20.CentralFeed;
using Uintra20.Core.Comments;
using Uintra20.Core.Groups;
using Uintra20.Core.Likes;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Subscribe;

namespace Uintra20.Core.Events
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
        public IEnumerable<Subscribe.Sql.Subscribe> Subscribers { get; set; }

        [JsonIgnore]
        public bool CanSubscribe { get; set; }
        [JsonIgnore]
        public string SubscribeNotes { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}