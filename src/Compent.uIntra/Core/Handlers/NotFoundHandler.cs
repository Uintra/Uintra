using Uintra.Core.Handlers;

namespace Compent.Uintra.Core.Handlers
{
    public class NotFoundHandler : NotFoundHandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}