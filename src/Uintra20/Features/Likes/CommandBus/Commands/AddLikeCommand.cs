using System;

namespace Uintra20.Features.Likes.CommandBus.Commands
{
    public class AddLikeCommand : LikeCommand
    {
        public AddLikeCommand(Guid entityId, Guid author) : base(entityId, author)
        {
        }
    }
}