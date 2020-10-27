using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Uintra.Core.Feed.Models;
using Uintra.Features.Comments.Models;
using Uintra.Features.Comments.Services;
using Uintra.Features.Groups;
using Uintra.Features.Likes;
using Uintra.Features.Likes.Models;

namespace Uintra.Features.News.Entities
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