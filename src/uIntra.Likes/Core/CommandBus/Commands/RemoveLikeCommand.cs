using System;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Likes.CommandBus
{
    public class RemoveLikeCommand : LikeCommand
    {
        public RemoveLikeCommand(ISingleLinkedList<ContextData> context, Guid author) : base(context, author)
        {
        }
    }
}