using System;
using Compent.CommandBus;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Likes.CommandBus
{
    public abstract class LikeCommand : ICommand
    {
        public SingleLinkedList<ContextData> Context { get; }
        public Guid Author { get; }

        protected LikeCommand(SingleLinkedList<ContextData> context, Guid author)
        {
            Context = context;
            Author = author;
        }
    }
}