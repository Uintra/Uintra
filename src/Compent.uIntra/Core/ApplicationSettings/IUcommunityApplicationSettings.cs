using System.Collections.Generic;
using uCommunity.Core.ApplicationSettings;

namespace Compent.uCommunity.Core.ApplicationSettings
{
    public interface IUcommunityApplicationSettings : IApplicationSettings
    {
        IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes { get; }
    }
}
