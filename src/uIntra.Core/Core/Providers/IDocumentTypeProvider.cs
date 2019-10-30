using System;

namespace Uintra.Core
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
        string GetBulletinsDetailsPage();
        string GetEventsDetailsPage();
        string GetNewsDetailsPage();

        string GetOverviewPage(Enum type);
        string GetEditPage(Enum type);
        string GetDetailsPage(Enum type);
        string GetCreatePage(Enum type);

        string GetMailTemplateFolder();
        string GetMailTemplate();
        string GetDataFolder();
        string GetUserTagFolder();
        string GetUserTag();
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
