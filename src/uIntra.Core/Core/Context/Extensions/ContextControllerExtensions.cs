using System;
using System.Web.Mvc;
using Uintra.Core.Context.Models;
using Uintra.Core.SingleLinkedList;
using static Uintra.Core.Context.ContextBuildActionType;
using static Uintra.Core.SingleLinkedList.SingleLinkedListExtensions;
using ContextData = Uintra.Core.Context.Models.ContextData;

namespace Uintra.Core.Context.Extensions
{
    public static class ContextControllerExtensions
    {
        public static SingleLinkedList<ControllerContextData> GetFullContext(this ControllerBase controller)
        {

            bool TryGetControllerContext(ControllerBase ctr, out ControllerContext ctrContext)
            {
                ctrContext = ctr.ControllerContext;
                return ctrContext != null;
            }

            SingleLinkedList<ControllerContextData> FullContextRec(ControllerBase ctrl)
            {
                switch (ctrl as ContextController)
                {
                    case ContextController contextController when
                        TryGetControllerContext(contextController, out var ctrContext) &&
                        ctrContext.ParentActionViewContext is ViewContext context:
                        return ToSingleLinkedList(contextController.GetControllerContextData(), FullContextRec(context.Controller));

                    case ContextController contextController when
                        TryGetControllerContext(contextController, out var ctrContext) &&
                        ctrContext.ParentActionViewContext is null:
                        return ToSingleLinkedList(contextController.GetControllerContextData());

                    case null when ctrl.ControllerContext != null && ctrl.ControllerContext.ParentActionViewContext is ViewContext context:
                        return FullContextRec(context.Controller);

                    default:
                        return Empty<ControllerContextData>();
                }
            }

            var result = FullContextRec(controller);
            return result.NormalizeContext();
        }


        public static ControllerContextData GetControllerContextData(this ContextController contextController) => 
            new ControllerContextData
        {
            ContextBuildActionType = contextController.ContextBuildActionType,
            СontextData = contextController.CtrlContextData
        };

        public static SingleLinkedList<ContextData> RecombineContext(SingleLinkedList<ControllerContextData> source)
        {
            SingleLinkedList<ContextData> ApplyAction(ControllerContextData action, SingleLinkedList<ContextData> appliedContext)
            {
                SingleLinkedList<ContextData> Subtracted() => appliedContext.Where(c => !action.СontextData.Equals(c));

                switch (action.ContextBuildActionType)
                {
                    case Add:
                        return ToSingleLinkedList(action.СontextData, appliedContext);
                    case Remove:
                        return Subtracted();
                    case Erasure:
                        return ToSingleLinkedList(action.СontextData, Subtracted());
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            return source.Catamorphism(
                node: ApplyAction,
                empty: Empty<ContextData>);
        }
    }
}