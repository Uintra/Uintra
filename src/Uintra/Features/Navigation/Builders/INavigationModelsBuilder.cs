using System.Collections.Generic;
using UBaseline.Core.Navigation;
using Uintra.Features.Navigation.Models;

namespace Uintra.Features.Navigation.Builders
{
    public interface INavigationModelsBuilder
    {
        IEnumerable<TreeNavigationItemModel> GetLeftSideNavigation();
        TopNavigationModel GetMobileNavigation();
        TopNavigationModel GetTopNavigationModel();
    }
}