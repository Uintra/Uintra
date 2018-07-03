using System;
using System.Web.Mvc;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Models;
using static Compent.Extensions.SingleLinkedList.SingleLinkedListExtensions;
using static Uintra.Core.Context.ContextBuildActionType;
using ContextData = Uintra.Core.Context.Models.ContextData;

namespace Uintra.Core.Context.Extensions
{
    public static class ContextControllerExtensions
    {
        public static ISingleLinkedList<ControllerContextData> GetFullContext(this ControllerBase controller)
        {

            bool TryGetControllerContext(ControllerBase ctr, out ControllerContext ctrContext)
            {
                ctrContext = ctr.ControllerContext;
                return ctrContext != null;
            }

            ISingleLinkedList<ControllerContextData> FullContextRec(ControllerBase ctrl)
            {
                switch (ctrl as ContextController)
                {
                    case ContextController contextController when
                        TryGetControllerContext(contextController, out var ctrContext) &&
                        ctrContext.ParentActionViewContext is ViewContext context:
                        return SingleLinkedList(contextController.GetControllerContextData(), FullContextRec(context.Controller));

                    case ContextController contextController when
                        TryGetControllerContext(contextController, out var ctrContext) &&
                        ctrContext.ParentActionViewContext is null:
                        return SingleLinkedList(contextController.GetControllerContextData());

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

        public static ISingleLinkedList<ContextData> RecombineContext(ISingleLinkedList<ControllerContextData> source)
        {
            ISingleLinkedList<ContextData> ApplyAction(ControllerContextData action, ISingleLinkedList<ContextData> appliedContext)
            {
                ISingleLinkedList<ContextData> Subtracted() => appliedContext.Where(c => !action.СontextData.Equals(c));

                switch (action.ContextBuildActionType)
                {
                    case Add:
                        return SingleLinkedList(action.СontextData, appliedContext);
                    case Remove:
                        return Subtracted();
                    case Erasure:
                        return SingleLinkedList(action.СontextData, Subtracted());
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