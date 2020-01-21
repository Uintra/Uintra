using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation.ModelBuilders.LeftSideMenu
{
    public interface ILeftSideNavigationModelBuilder
    {
        MenuModel GetMenu();
        UserListLinkModel GetUserListLink();
    }
}
