using System;
using Compent.CommandBus;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Likes.CommandBus
{
    public abstract class LikeCommand : ICommand
    {
        public ISingleLinkedList<ContextData> Context { get; }
        public Guid Author { get; }

        protected LikeCommand(ISingleLinkedList<ContextData> context, Guid author)
        {
            Context = context;
            Author = author;
        }
    }
}