using Compent.CommandBus;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Comments.CommandBus
{
    public abstract class CommentCommand : ICommand
    {
        public SingleLinkedList<ContextData> Context { get; }

        protected CommentCommand(SingleLinkedList<ContextData> context)
        {
            Context = context;
        }
    }
}