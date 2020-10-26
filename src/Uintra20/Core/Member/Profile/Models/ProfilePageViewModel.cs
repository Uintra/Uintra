﻿using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Core.Member.Profile.Models
{
    public class ProfilePageViewModel : UintraRestrictedNodeViewModel
    {
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public ProfileViewModel Profile { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public UintraLinkModel EditProfileLink { get; set; }
    }
}