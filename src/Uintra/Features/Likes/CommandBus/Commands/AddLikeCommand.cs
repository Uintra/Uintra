using System;

namespace Uintra.Features.Likes.CommandBus.Commands
{
    public class AddLikeCommand : LikeCommand
    {
        public AddLikeCommand(Guid entityId, Enum entityType, Guid author) : base(entityId, entityType, author)
        {
        }
    }
}