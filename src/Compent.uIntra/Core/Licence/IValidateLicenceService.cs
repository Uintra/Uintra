using System;

namespace Compent.uIntra.Core.Licence
{
    public interface IValidateLicenceService
    {
        Lazy<bool> IsLicenceValid { get; }
    }
}