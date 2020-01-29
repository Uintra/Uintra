﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups;
using Uintra20.Features.Likes;
using Uintra20.Features.Likes.Models;

namespace Uintra20.Features.News.Entities
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