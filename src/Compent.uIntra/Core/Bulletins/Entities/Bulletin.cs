using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Bulletins
{
    public class Bulletin : BulletinBase, IFeedItem, ICommentable, ILikeable, ISubscribable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<CommentModel> Comments { get; set; }

        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}