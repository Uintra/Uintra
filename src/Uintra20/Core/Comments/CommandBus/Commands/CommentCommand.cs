using System;
using Compent.CommandBus;

namespace Uintra20.Core.Comments.CommandBus
{
    public abstract class CommentCommand : ICommand
    {
        public Guid TargetId { get; set; }
        public Enum TargetType { get; set; }

        protected CommentCommand(Guid targetId, Enum targetType)
        {
            TargetId = targetId;
            TargetType = targetType;
        }
    }
}