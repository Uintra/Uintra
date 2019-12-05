using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups;
using Uintra20.Features.Likes;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Subscribe;

namespace Uintra20.Features.Events.Entities
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