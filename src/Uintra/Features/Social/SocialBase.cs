using Newtonsoft.Json;
using System;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Member.Abstractions;
using Uintra.Features.LinkPreview.Contracts;
using Uintra.Features.LinkPreview.Models;

namespace Uintra.Features.Social
{
    public class SocialBase : IntranetActivity, IHaveCreator, IHaveOwner, ISocialBase, IHasLinkPreview
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }

        [JsonIgnore]
        public LinkPreviewModel LinkPreview { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}