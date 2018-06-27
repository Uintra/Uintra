using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Comments.CommandBus
{
    public class AddCommentCommand : CommentCommand
    {
        public CommentCreateDto CreateDto { get; }

        public AddCommentCommand(ISingleLinkedList<ContextData> context, CommentCreateDto createDto) : base(context)
        {
            CreateDto = createDto;
        }
    }
}