using System;
using Compent.CommandBus;

namespace Uintra20.Core.Likes.CommandBus
{
    public abstract class LikeCommand : ICommand
    {
        public Guid Author { get; }
        public Guid EntityId { get; }

        protected LikeCommand(Guid entityId, Guid author)
        {
            Author = author;
            EntityId = entityId;
        }
    }
}