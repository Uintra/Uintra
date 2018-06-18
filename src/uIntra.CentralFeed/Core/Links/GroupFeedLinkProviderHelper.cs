using System.Collections.Generic;
using Compent.Extensions;
using Uintra.Core;

namespace Uintra.CentralFeed
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider)
        {
            return aliasProvider.GetHomePage().ToEnumerable();
        }
    }
}
