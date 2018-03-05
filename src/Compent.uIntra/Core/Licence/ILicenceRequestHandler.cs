using System;
namespace Compent.Uintra.Core.Licence
{
    public interface ILicenceRequestHandler
    {
        void BeginRequestHandler(object obj, EventArgs args);
    }
}