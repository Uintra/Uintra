using HandlerBase = uCommunity.Core.Handlers.NotFoundHandlerBase;

namespace Compent.uCommunity.Core.Handlers
{
    public class NotFoundHandler: HandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}