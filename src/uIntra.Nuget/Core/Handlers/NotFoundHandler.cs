namespace uIntra.Core.Handlers
{
    public class NotFoundHandler: NotFoundHandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}