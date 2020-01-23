using UBaseline.Core.Navigation;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation
{
    public interface INavigationModelsBuilder
    {
        IEnumerable<TreeNavigationItemModel> GetLeftSideNavigation();
        TopNavigationModel GetTopNavigationModel();
    }
}