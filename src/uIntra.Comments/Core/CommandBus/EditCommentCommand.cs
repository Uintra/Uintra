using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Comments.CommandBus
{
    public class EditCommentCommand : CommentCommand
    {
        public CommentEditDto EditDto { get; }

        public EditCommentCommand(SingleLinkedList<ContextData> context, CommentEditDto editDto) : base(context)
        {
            EditDto = editDto;
        }
    }
}