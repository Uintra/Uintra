using System;

namespace Uintra.Features.Likes.CommandBus.Commands
{
    public class RemoveLikeCommand : LikeCommand
    {
        public RemoveLikeCommand(Guid entityId, Enum entityType, Guid author) : base(entityId, entityType, author)
        {
        }
    }
}