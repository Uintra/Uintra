using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Context.Extensions
{
    public static class ContextExtensions
    {

        public static TResult Catamorphism<TResult>(
            this ISingleLinkedList<ContextData> context,
            Func<TResult> empty,
            params (Enum contextType, Func<ContextData, TResult, TResult> caseFunc)[] cases)
        {
            TResult CatamorphismRec(ISingleLinkedList<ContextData> node) =>
                node.Match(
                    Case(ContextType.Any, (contextData, subContext) =>
                        cases
                            .First(caseFunc => caseFunc.contextType.HasFlag(contextData.Type))
                            .caseFunc(contextData, CatamorphismRec(subContext))),

                    Case(ContextType.Empty, (x, y) => empty()));

            return CatamorphismRec(context);
        }

        public static TResult Match<TResult>(
            this ISingleLinkedList<ContextData> context,
            params ContextMatchCase<TResult>[] cases)
        {
            bool IsForEmptyCase(ContextMatchCase<TResult> caseFunc) =>
                ExactScalar(ContextType.Empty, caseFunc.Type);

            bool IsForCase(ContextMatchCase<TResult> caseFunc) =>
                caseFunc.Type.HasFlag(context.Value.Type);

            var isEmptyType = ExactScalar(ContextType.Empty, context.Value.Type);

            return cases
                .First(caseFunc => isEmptyType
                    ? IsForEmptyCase(caseFunc)
                    : IsForCase(caseFunc))
                .Func(context.Value, context.Tail);
        }

        public static ISingleLinkedList<ControllerContextData> NormalizeContext(this ISingleLinkedList<ControllerContextData> context)
        {
            bool IsCommentSequenceInitalContext(ContextData contextData) =>
                ExactScalar(contextData.Type, ContextType.Comment) && contextData.EntityId is null;

            return context.Catamorphism(
                node: (contextData, subContext) =>
                    subContext != null && contextData.Equals(subContext.Value) || IsCommentSequenceInitalContext(contextData.СontextData)
                        ? subContext
                        : SingleLinkedListExtensions.SingleLinkedList(contextData, subContext),

                empty: SingleLinkedListExtensions.Empty<ControllerContextData>);
        }

        public static ContextDataTransferModel ContextDataTransferModel(ContextData contextData) =>
            new ContextDataTransferModel
            {
                TypeId = contextData.Type.ToInt(),
                EntityId = contextData.EntityId
            };

        public static ContextData ContextData(ContextDataTransferModel model, IEnumerable<Enum> allContextTypes) =>
            ContextData(allContextTypes.Get(model.TypeId), model.EntityId);

        public static ISingleLinkedList<TResult> Select<T, TResult>(
            this ISingleLinkedList<T> context,
            Func<T, TResult> func) =>
            context.Catamorphism(
                node: (item, mappedResult) => SingleLinkedListExtensions.SingleLinkedList(func(item), mappedResult),
                empty: SingleLinkedListExtensions.Empty<TResult>);

        public static ContextData ContextData(Enum type, Guid? entityId = null) =>
            new ContextData
            {
                Type = type,
                EntityId = entityId
            };

        public static ContextDataTransferModel ToDto(this ContextData contextData) =>
            new ContextDataTransferModel
            {
                TypeId = contextData.Type.ToInt(),
                EntityId = contextData.EntityId
            };

        public static bool HasFlagScalar(Enum a, Enum b) => HasFlagScalar(a.ToInt(), b.ToInt());

        public static bool HasFlagScalar(Enum a, int b) => HasFlagScalar(a.ToInt(), b);

        public static bool HasFlagScalar(int a, Enum b) => HasFlagScalar(a, b.ToInt());

        public static bool HasFlagScalar(int a, int b) => (a & b) != 0;

        public static bool ExactScalar(Enum a, Enum b) => a.ToInt() == b.ToInt();

        public static ContextMatchCase<TResult> Case<TResult>(Enum type, Func<ContextData, ISingleLinkedList<ContextData>, TResult> func) =>
            new ContextMatchCase<TResult>
            {
                Type = type,
                Func = func
            };
    }

    public class ContextMatchCase<TResult>
    {
        public Enum Type { get; set; }
        public Func<ContextData, ISingleLinkedList<ContextData>, TResult> Func { get; set; }
    }
}