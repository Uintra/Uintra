using Uintra.Core.Handlers;

namespace Compent.Uintra.Core.Handlers
{
    // This file must be Compile in uInta nuget package !!!
    public class NotFoundHandler : NotFoundHandlerBase
    {
        protected override string ErrorPageDocType { get; } = "errorPage";
    }
}