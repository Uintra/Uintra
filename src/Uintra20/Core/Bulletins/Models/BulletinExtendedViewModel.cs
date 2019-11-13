﻿using Uintra20.Core.Comments;
using Uintra20.Core.Likes;

namespace Uintra20.Core.Bulletins
{
    public class BulletinExtendedViewModel : BulletinViewModel
    {
        public ILikeable LikesInfo { get; set; }
        public ICommentable CommentsInfo { get; set; }
    }
}