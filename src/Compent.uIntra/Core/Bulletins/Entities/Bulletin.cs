using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra.Bulletins;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Groups;
using Uintra.Likes;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.Bulletins
{
    public class Bulletin : BulletinBase, IFeedItem, ICommentable, ILikeable, ISubscribable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<CommentModel> Comments { get; set; }

        public IEnumerable<global::Uintra.Subscribe.Subscribe> Subscribers { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}