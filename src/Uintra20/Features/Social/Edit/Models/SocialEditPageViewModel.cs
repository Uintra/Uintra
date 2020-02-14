using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Shared.Node;
using UBaseline.Shared.PageSettings;
using UBaseline.Shared.Property;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Social.Edit.Models
{
    public class SocialEditPageViewModel : NodeViewModel
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
        public bool CanEdit { get; set; }
    }
}