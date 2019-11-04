using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra20.CentralFeed;
using Uintra20.Core.Comments;
using Uintra20.Core.Groups;
using Uintra20.Core.Likes;

namespace Uintra20.Core.Bulletins
{
    public class Bulletin : BulletinBase, IFeedItem, ICommentable, ILikeable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<CommentModel> Comments { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }
    }
}