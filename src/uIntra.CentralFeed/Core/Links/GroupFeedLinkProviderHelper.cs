using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Extensions;

namespace uIntra.CentralFeed
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider)
        {
            return aliasProvider.GetHomePage().ToEnumerableOfOne();
        }
    }
}
