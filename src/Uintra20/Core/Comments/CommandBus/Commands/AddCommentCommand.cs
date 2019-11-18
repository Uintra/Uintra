using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Core.Comments.CommandBus
{
    public class AddCommentCommand : CommentCommand
    {
        public CommentCreateDto CreateDto { get; }

        public AddCommentCommand(Guid targetId, Enum targetType, CommentCreateDto createDto) : base(targetId, targetType)
        {
            CreateDto = createDto;
        }
    }
}