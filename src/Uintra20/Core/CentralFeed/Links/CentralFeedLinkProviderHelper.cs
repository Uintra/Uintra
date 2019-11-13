using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Uintra20.Core.CentralFeed
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider) =>
            List(aliasProvider.GetHomePage());
    }
}