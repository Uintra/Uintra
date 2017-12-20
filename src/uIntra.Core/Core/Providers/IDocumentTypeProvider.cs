using uIntra.Core.TypeProviders;

namespace uIntra.Core
{
    public interface IDocumentTypeAliasProvider
    {
        string GetNavigationComposition();
        string GetContentPage();
        string GetHeading();
        string GetSearchResultPage();
        string GetProfilePage();
        string GetProfileEditPage();
        string GetNotificationPage();
        string GetHomePage();
        string GetErrorPage();

        string GetOverviewPage(IIntranetType type);
        string GetEditPage(IIntranetType type);
        string GetDetailsPage(IIntranetType type);
        string GetCreatePage(IIntranetType type);

        string GetMailTemplateFolder();
        string GetMailTemplate();
        string GetDataFolder();
        string GetSystemLink();
        string GetSystemLinkFolder();

        string GetGroupOverviewPage();
        string GetGroupCreatePage();
        string GetGroupRoomPage();
        string GetGroupEditPage();
        string GetGroupMyGroupsOverviewPage();
        string GetGroupDeactivatedPage();
    }
}
