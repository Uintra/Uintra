using System;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Comments.CommandBus
{
    public class RemoveCommentCommand : CommentCommand
    {
        public Guid CommentId { get; }

        public RemoveCommentCommand(SingleLinkedList<ContextData> context, Guid commentId) : base(context)
        {
            CommentId = commentId;
        }
    }
}