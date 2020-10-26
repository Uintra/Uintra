using Uintra.Infrastructure.Providers;

namespace Uintra.Features.CentralFeed.Links
{
    public static class CentralFeedLinkProviderHelper
    {
        public static string GetFeedActivitiesAlias(IDocumentTypeAliasProvider aliasProvider) =>
            aliasProvider.GetHomePage();
    }
}