using System;

namespace Uintra20.Core.Likes.CommandBus
{
    public class RemoveLikeCommand : LikeCommand
    {
        public RemoveLikeCommand(Guid entityId, Guid author) : base(entityId, author)
        {
        }
    }
}