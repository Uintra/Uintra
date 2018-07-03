using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Comments.CommandBus
{
    public class EditCommentCommand : CommentCommand
    {
        public CommentEditDto EditDto { get; }

        public EditCommentCommand(ISingleLinkedList<ContextData> context, CommentEditDto editDto) : base(context)
        {
            EditDto = editDto;
        }
    }
}