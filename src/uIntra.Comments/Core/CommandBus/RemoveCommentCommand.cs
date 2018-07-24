using System;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Comments.CommandBus
{
    public class RemoveCommentCommand : CommentCommand
    {
        public Guid CommentId { get; }

        public RemoveCommentCommand(ISingleLinkedList<ContextData> context, Guid commentId) : base(context)
        {
            CommentId = commentId;
        }
    }
}