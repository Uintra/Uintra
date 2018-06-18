using System;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Likes.CommandBus
{
    public class RemoveLikeCommand : LikeCommand
    {
        public RemoveLikeCommand(SingleLinkedList<ContextData> context, Guid author) : base(context, author)
        {
        }
    }
}