using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Comments.CommandBus
{
    public class AddCommentCommand : CommentCommand
    {
        public CommentCreateDto CreateDto { get; }

        public AddCommentCommand(SingleLinkedList<ContextData> context, CommentCreateDto createDto) : base(context)
        {
            CreateDto = createDto;
        }
    }
}