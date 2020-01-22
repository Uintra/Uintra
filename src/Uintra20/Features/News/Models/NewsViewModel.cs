using System;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes;

namespace Uintra20.Features.News.Models
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}