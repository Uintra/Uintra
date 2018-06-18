using System.Collections.Generic;
using Uintra.Core;

namespace Uintra.Groups
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
