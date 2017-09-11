using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using uIntra.Bulletins;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Bulletins
{
    public class Bulletin : BulletinBase, IFeedItem, ICommentable, ILikeable, ISubscribable, IGroupable
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}