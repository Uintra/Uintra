using uCommunity.Core.Handlers;

namespace Compent.uCommunity.Core.Handlers
{
    public class NotFoundHandler: NotFoundHandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}