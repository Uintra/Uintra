﻿using System;

namespace Uintra20.Core.Comments.CommandBus
{
    public class RemoveCommentCommand : CommentCommand
    {
        public Guid CommentId { get; }

        public RemoveCommentCommand(Guid targetId, Enum targetType, Guid commentId) : base(targetId, targetType)
        {
            CommentId = commentId;
        }
    }
}