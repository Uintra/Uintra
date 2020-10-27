﻿using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.UbaselineModels.RestrictedNode;

namespace Uintra.Features.Groups.Models
{
    public class UintraGroupsCreatePageViewModel : UintraRestrictedNodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public GroupLeftNavigationMenuViewModel Navigation { get; set; }
    }
}