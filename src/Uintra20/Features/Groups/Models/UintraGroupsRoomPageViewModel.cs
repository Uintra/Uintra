﻿using System;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;

namespace Uintra20.Features.Groups.Models
{
    public class UintraGroupsRoomPageViewModel : NodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public GroupNavigationCompositionViewModel GroupNavigation { get; set; }
        public Guid? GroupId { get; set; }
        public GroupViewModel GroupInfo { get; set; }
    }
}