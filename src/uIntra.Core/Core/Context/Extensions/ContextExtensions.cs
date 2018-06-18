using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Context.Models;
using Uintra.Core.Extensions;
using Uintra.Core.SingleLinkedList;
using Uintra.Core.TypeProviders;
using static Uintra.Core.SingleLinkedList.SingleLinkedListExtensions;

namespace Uintra.Core.Context
{
    public static class ContextExtensions
    {

        public static TResult Catamorphism<TResult>(
            this SingleLinkedList<ContextData> context,
            Func<TResult> empty,
            params (Enum contextType, Func<ContextData, TResult, TResult> caseFunc)[] cases)
        {
            TResult CatamorphismRec(SingleLinkedList<ContextData> node) =>
                node.Match(
                    Case(ContextType.Any, (contextData, subContext) =>
                        cases
                            .First(caseFunc => caseFunc.contextType.HasFlag(contextData.Type))
                            .caseFunc(contextData, CatamorphismRec(subContext))),

                    Case(ContextType.Empty, (x, y) => empty()));

            return CatamorphismRec(context);
        }

        public static TResult Match<TResult>(
            this SingleLinkedList<ContextData> context,
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

        public static SingleLinkedList<ControllerContextData> NormalizeContext(this SingleLinkedList<ControllerContextData> context)
        {
            bool IsCommentSequenceInitalContext(ContextData contextData) =>
                ExactScalar(contextData.Type, ContextType.Comment) && contextData.EntityId is null;

            return context.Catamorphism(
                node: (contextData, subContext) =>
                    subContext != null && contextData.Equals(subContext.Value) || IsCommentSequenceInitalContext(contextData.СontextData)
                        ? subContext
                        : ToSingleLinkedList(contextData, subContext),

                empty: Empty<ControllerContextData>);
        }

        public static ContextDataTransferModel ContextDataTransferModel(ContextData contextData) =>
            new ContextDataTransferModel
            {
                TypeId = contextData.Type.ToInt(),
                EntityId = contextData.EntityId
            };

        public static ContextData ContextData(ContextDataTransferModel model, IEnumerable<Enum> allContextTypes) =>
            ContextData(allContextTypes.Get(model.TypeId), model.EntityId);

        public static SingleLinkedList<TResult> Select<T, TResult>(
            this SingleLinkedList<T> context,
            Func<T, TResult> func) =>
            context.Catamorphism(
                node: (item, mappedResult) => ToSingleLinkedList(func(item), mappedResult),
                empty: Empty<TResult>);

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

        public static ContextMatchCase<TResult> Case<TResult>(Enum type, Func<ContextData, SingleLinkedList<ContextData>, TResult> func) =>
            new ContextMatchCase<TResult>
            {
                Type = type,
                Func = func
            };
    }

    public class ContextMatchCase<TResult>
    {
        public Enum Type { get; set; }
        public Func<ContextData, SingleLinkedList<ContextData>, TResult> Func { get; set; }
    }
}