using uIntra.Core.TypeProviders;

namespace uIntra.Core
{
    public interface IDocumentTypeAliasProvider
    {
        string Get(string alias);
        string GetNavigationComposition();
        string GetContentPage();
        string GetSearchResultPage();
        string GetProfilePage();
        string GetProfileEditPage();
        string GetNotificationPage();
        string GetHomePage();

        string GetOverviewPage(IIntranetType type);
        string GetEditPage(IIntranetType type);
        string GetDetailsPage(IIntranetType type);
        string GetCreatePage(IIntranetType type);

        string GetMailTemplateFolder();
        string GetMailTemplate();
        string GetDataFolder();
        string GetSystemLink();
        string GetSystemLinkFolder();
    }
}
