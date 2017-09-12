using System;
namespace Compent.uIntra.Core.Licence
{
    public interface ILicenceRequestHandler
    {
        void BeginRequestHandler(object obj, EventArgs args);
    }
}