using System.Collections.Generic;
using uIntra.Core.ApplicationSettings;

namespace Compent.uIntra.Core.ApplicationSettings
{
    public interface IuIntraApplicationSettings : IApplicationSettings
    {
        IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes { get; }
    }
}
