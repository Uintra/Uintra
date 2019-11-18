using System;

namespace Uintra20.Core.Likes.CommandBus
{
    public class AddLikeCommand : LikeCommand
    {
        public AddLikeCommand(Guid entityId, Guid author) : base(entityId, author)
        {
        }
    }
}