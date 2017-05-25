using System.Collections.Generic;
using uCommunity.Core.ApplicationSettings;

namespace Compent.uIntra.Core.ApplicationSettings
{
    public interface IUcommunityApplicationSettings : IApplicationSettings
    {
        IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes { get; }
    }
}
