using System;
using Newtonsoft.Json;
using Uintra20.Features.Activity.Entities;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.User;

namespace Uintra20.Features.Bulletins
{
    public class BulletinBase : IntranetActivity, IHaveCreator, IHaveOwner, IBulletinBase, IHasLinkPreview
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