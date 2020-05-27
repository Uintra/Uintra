﻿using System.Collections.Generic;
using UBaseline.Core.Navigation;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.Navigation.Builders
{
    public interface INavigationModelsBuilder
    {
        IEnumerable<TreeNavigationItemModel> GetLeftSideNavigation();
        TopNavigationModel GetMobileNavigation();
        TopNavigationModel GetTopNavigationModel();
    }
}