using System.Collections.Generic;
using Uintra.Core;
using static LanguageExt.Prelude;

namespace Uintra.CentralFeed
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider) => 
            List(aliasProvider.GetHomePage());
    }
}
