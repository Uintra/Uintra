using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Features.Groups;
using Uintra.Features.Groups.Models;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Links.Models;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Social.Models
{
    public class SocialEditPageViewModel : UintraRestrictedNodeViewModel, IGroupHeader
    {
        public Guid OwnerId { get; set; }
        public PropertyViewModel<INodeViewModel[]> Panels { get; set; }
        public PageSettingsCompositionViewModel PageSettings { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserTag> Tags { get; set; } = Enumerable.Empty<UserTag>();
        public LightboxPreviewModel LightboxPreviewModel { get; set; }
        public IEnumerable<UserTag> AvailableTags { get; set; } = Enumerable.Empty<UserTag>();
        public Guid Id { get; set; }
        public string AllowedMediaExtensions { get; set; }
        public bool CanDelete { get; set; }
        public IActivityLinks Links { get; set; }
        public GroupHeaderViewModel GroupHeader { get; set; }
    }
}