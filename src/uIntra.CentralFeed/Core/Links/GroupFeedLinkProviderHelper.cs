using System.Collections.Generic;
using Extensions;
using uIntra.Core;

namespace uIntra.CentralFeed
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider)
        {
            return aliasProvider.GetHomePage().ToEnumerable();
        }
    }
}
