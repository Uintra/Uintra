using Uintra20.Infrastructure.Providers;

namespace Uintra20.Features.CentralFeed.Links
{
    public static class CentralFeedLinkProviderHelper
    {
        public static string GetFeedActivitiesAlias(IDocumentTypeAliasProvider aliasProvider) =>
            aliasProvider.GetHomePage();
    }
}