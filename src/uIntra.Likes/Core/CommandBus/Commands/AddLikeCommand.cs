using System;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Likes.CommandBus
{
    public class AddLikeCommand : LikeCommand
    {
        public AddLikeCommand(SingleLinkedList<ContextData> context, Guid author) : base(context, author)
        {
        }
    }
}