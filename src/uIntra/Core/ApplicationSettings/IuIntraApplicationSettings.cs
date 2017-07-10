using System.Collections.Generic;

namespace uIntra.Core.ApplicationSettings
{
    public interface IuIntraApplicationSettings : IApplicationSettings
    {
        IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes { get; }
    }
}
