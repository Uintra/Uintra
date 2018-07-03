using System;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;

namespace Uintra.Likes.CommandBus
{
    public class AddLikeCommand : LikeCommand
    {
        public AddLikeCommand(ISingleLinkedList<ContextData> context, Guid author) : base(context, author)
        {
        }
    }
}