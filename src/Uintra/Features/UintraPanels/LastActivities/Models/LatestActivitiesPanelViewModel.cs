﻿using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;
using Uintra.Core.Feed.Models;

namespace Uintra.Features.UintraPanels.LastActivities.Models
{
    public class LatestActivitiesPanelViewModel : NodeViewModel
    {
        public PropertyViewModel<string> Title { get; set; }
        public PropertyViewModel<string> Teaser { get; set; }
        public PropertyViewModel<SelectedActivityViewModel> ActivityType { get; set; }
        public PropertyViewModel<int> CountToDisplay { get; set; }
        public IEnumerable<LatestActivitiesItemViewModel> Feed { get; set; }
        public bool ShowSeeAllButton { get; set; }
    }
}