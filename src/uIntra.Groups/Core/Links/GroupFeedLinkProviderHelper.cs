using System.Collections.Generic;
using uIntra.Core;

namespace uIntra.Groups
{
    public static class GroupFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider)
        {
            return new[]
            {
                aliasProvider.GetHomePage(),
                aliasProvider.GetGroupOverviewPage(),
                aliasProvider.GetGroupRoomPage()
            };
        }
    }
}
