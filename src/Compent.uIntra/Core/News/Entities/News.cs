using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Groups;
using Uintra.Likes;
using Uintra.News;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.News.Entities
{
    public class News : NewsBase, IFeedItem, ICommentable, ILikeable, IGroupActivity
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