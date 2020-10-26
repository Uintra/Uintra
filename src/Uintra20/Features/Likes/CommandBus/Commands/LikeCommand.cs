using System;
using Compent.CommandBus;

namespace Uintra20.Features.Likes.CommandBus.Commands
{
    public abstract class LikeCommand : ICommand
    {
        public Guid Author { get; }
        public Guid EntityId { get; }
        public Enum EntityType { get; }

        protected LikeCommand(Guid entityId, Enum entityType, Guid author)
        {
            Author = author;
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}