using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra.Core.Feed.Models;
using Uintra.Features.Comments.Models;
using Uintra.Features.Comments.Services;
using Uintra.Features.Groups;
using Uintra.Features.Likes;
using Uintra.Features.Likes.Models;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Subscribe;

namespace Uintra.Features.Events.Entities
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