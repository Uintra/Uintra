using Compent.CommandBus;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Comments.CommandBus
{
    public abstract class CommentCommand : ICommand
    {
        public ISingleLinkedList<ContextData> Context { get; }

        protected CommentCommand(ISingleLinkedList<ContextData> context)
        {
            Context = context;
        }
    }
}