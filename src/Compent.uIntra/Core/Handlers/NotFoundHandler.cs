using uIntra.Core.Handlers;

namespace Compent.uIntra.Core.Handlers
{
    // This file must be Compile in uInta nuget package !!!
    public class NotFoundHandler : NotFoundHandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}