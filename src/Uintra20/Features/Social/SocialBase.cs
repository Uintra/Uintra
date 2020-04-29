using Newtonsoft.Json;
using System;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Features.LinkPreview.Contracts;

namespace Uintra20.Features.Social
{
    public class SocialBase : IntranetActivity, IHaveCreator, IHaveOwner, ISocialBase, IHasLinkPreview
    {
        public int? UmbracoCreatorId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime PublishDate { get; set; }

        [JsonIgnore]
        public LinkPreview.Models.LinkPreview LinkPreview { get; set; }

        public int? LinkPreviewId { get; set; }
    }
}