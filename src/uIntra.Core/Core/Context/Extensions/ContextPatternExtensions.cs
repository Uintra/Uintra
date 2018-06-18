using System;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;

namespace Uintra.Core.Context
{
    public static class ContextPatternExtensions
    {
        public static bool StartsWith(this ContextData contextData, Enum firstPtr) =>
            ContextExtensions.HasFlagScalar(firstPtr, contextData.Type);

        public static bool StartsWith(
            this SingleLinkedList<ContextData> context,
            Enum firstPtr,
            out ContextData first)
        {
            if (context != null && context.Value.StartsWith(firstPtr))
            {
                first = context.Value;
                return true;
            }

            first = null;
            return false;
        }

        public static bool StartsWith(
            this SingleLinkedList<ContextData> context,
            Enum firstPtr) =>
            context.StartsWith(firstPtr, out _);


        public static bool StartsWith(
            this SingleLinkedList<ContextData> context,
            Enum firstPtr,
            out ContextData first,
            out SingleLinkedList<ContextData> tail)
        {
            if (context.StartsWith(firstPtr, out first))
            {
                tail = context.Tail;
                return true;
            }

            tail = null;
            return true;
        }

        public static bool StartsWith(
            this SingleLinkedList<ContextData> context,
            Enum firstPtr, Enum secondPtr,
            out ContextData first, out ContextData second,
            out SingleLinkedList<ContextData> tail)
        {
            if (context.StartsWith(firstPtr, out first, out var rest) && rest.StartsWith(secondPtr, out second, out tail))
            {
                return true;
            }

            first = null;
            second = null;
            tail = null;
            return false;
        }

        public static bool StartsWith(
            this SingleLinkedList<ContextData> context,
            Enum firstPtr, Enum secondPtr, Enum thirdPtr,
            out ContextData first, out ContextData second, out ContextData third,
            out SingleLinkedList<ContextData> tail)
        {
            if (context.StartsWith(firstPtr, secondPtr, out first, out second, out var rest) && rest.StartsWith(thirdPtr, out third, out tail))
            {
                return true;
            }

            first = null;
            second = null;
            third = null;
            tail = null;
            return false;
        }

        public static bool StartsWithCommentsSequence(
            this SingleLinkedList<ContextData> context,
            out Guid topCommentId,
            out SingleLinkedList<ContextData> comments,
            out ContextData commentsTarget,
            out SingleLinkedList<ContextData> tail)
        {
            var (commentsResult, rest) = context.Span(c => ContextExtensions.ExactScalar(c.Type, ContextType.Comment));

            if (commentsResult.Any())
            {
                topCommentId = commentsResult.Value.EntityId.Value;
                comments = commentsResult.Tail;
                commentsTarget = rest.Value;
                tail = rest.Tail;

                return true;
            }

            topCommentId = Guid.Empty;
            comments = null;
            commentsTarget = null;
            tail = null;
            return false;
        }

        public static ContextData GetCommentsTarget(
            this SingleLinkedList<ContextData> context)
        {
            var result = context.SkipWhile(c => ContextExtensions.ExactScalar(c.Type, ContextType.Comment));
            return result.Value;
        }
    }
}