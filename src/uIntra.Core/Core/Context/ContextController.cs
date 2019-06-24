using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Compent.Extensions;
using Compent.Extensions.SingleLinkedList;
using Uintra.Core.Context.Extensions;
using Uintra.Core.Context.Models;
using Uintra.Core.Extensions;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Context
{
    public abstract class ContextController : ContextedController
    {
        public abstract Enum ControllerContextType { get; }

        public virtual ContextBuildActionType ContextBuildActionType { get; set; } = ContextBuildActionType.Erasure;

        public ContextData CtrlContextData { get; set; }

        protected ContextController(IContextTypeProvider contextTypeProvider) : base(contextTypeProvider)
        {
            CtrlContextData = new ContextData
            {
                Type = ControllerContextType
            };
        }

        protected void AddEntityIdentityForContext(Guid entityId, Enum controllerContextType = null)
        {
            CtrlContextData = new ContextData
            {
                Type = controllerContextType ?? CtrlContextData.Type,
                EntityId = entityId
            };

            UpdateViewData();
        }
    }

    public abstract class ContextedController : SurfaceController
    {
        private readonly IContextTypeProvider _contextTypeProvider;
        private const string ContextKey = "context";

        protected ContextedController(IContextTypeProvider contextTypeProvider)
        {
            _contextTypeProvider = contextTypeProvider;
        }

        public ISingleLinkedList<ContextData> FullContext
        {
            get
            {
                var request = Request.Unvalidated();

                IEnumerable<ContextData> ParseContext(string contextJson) => contextJson
                    .Bind(SerializationExtensions.Deserialize<IEnumerable<ContextDataTransferModel>>)
                    .Select(data => ContextExtensions.ContextData(data, _contextTypeProvider.All));

                IEnumerable<ContextData> TryGetContextFromRequest() => request
                    .QueryString[ContextKey]
                    .DefaultWith(() => request.Form[ContextKey])
                    .Bind(ParseContext);

                var requestContext =
                    TryGetContextFromRequest()
                        .DefaultWith(Enumerable.Empty<ContextData>)
                        .Select(data => new ControllerContextData
                        {
                            СontextData = data,
                            ContextBuildActionType = ContextBuildActionType.Add
                        });

                var viewContext = this.GetFullContext().DefaultWith(Enumerable.Empty<ControllerContextData>);

                return viewContext
                    .Concat(requestContext)
                    .Bind(SingleLinkedListExtensions.ToSingleLinkedList)
                    .Bind(ContextControllerExtensions.RecombineContext);
            }
        }

        public void UpdateViewData()
        {
            ViewData[ContextKey] = ContextExtensions.Select(FullContext
                    .Where(x => !ContextExtensions.ExactScalar(x.Type, ContextType.Empty)), ContextExtensions.ContextDataTransferModel);
        }
    }
}