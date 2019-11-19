using System.Collections.Generic;
using Uintra20.Infrastructure.Providers;
using static LanguageExt.Prelude;

namespace Uintra20.Features.CentralFeed.Links
{
    public static class CentralFeedLinkProviderHelper
    {
        public static IEnumerable<string> GetFeedActivitiesXPath(IDocumentTypeAliasProvider aliasProvider) =>
            List(aliasProvider.GetHomePage());
    }
}