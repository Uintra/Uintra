using System;

namespace Uintra20.Features.Likes.CommandBus.Commands
{
    public class RemoveLikeCommand : LikeCommand
    {
        public RemoveLikeCommand(Guid entityId, Enum entityType, Guid author) : base(entityId, entityType, author)
        {
        }
    }
}