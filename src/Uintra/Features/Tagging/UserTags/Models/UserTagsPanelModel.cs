﻿using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.Property;

namespace Uintra.Features.Tagging.UserTags.Models
{
    public class UserTagsPanelModel : NodeModel
    {
        public PropertyModel<IEnumerable<UserTagPanelModel>> Tags { get; set; }
    }
}